using Lopes.SC.Commons;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Application.Models;
using Lopes.SC.ExportacaoAnuncio.Application.Services.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using System.Collections.Concurrent;


namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{
    public class AtualizarAnunciosAppService : IAtualizarAnunciosAppService
    {
        private readonly ILogger _logger;
        private readonly IDadosImovelAppService _dadosImovelAppService;
        private readonly IAnuncioAppService _anuncioAppService;
        private readonly IImovelAtualizacaoPortaisRepository _imovelAtualizacaoPortaisRepository;
        private readonly IStatusAnuncioService _statusAnuncioService;
        private readonly IImovelXMLAppService _imovelXMLAppService;

        private readonly List<Imovel> _dadosImoveisCache;

        public AtualizarAnunciosAppService(ILogger logger,
                                           IAnuncioAppService anuncioAppService,
                                           IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository,
                                           IDadosImovelAppService dadosImovelAppService,
                                           IStatusAnuncioService statusAnuncioService,
                                           IImovelXMLAppService imovelXMLAppService)
        {
            _logger = logger;
            _anuncioAppService = anuncioAppService;
            _imovelAtualizacaoPortaisRepository = imovelAtualizacaoPortaisRepository;
            _dadosImoveisCache = new List<Imovel>();
            _dadosImovelAppService = dadosImovelAppService;
            _statusAnuncioService = statusAnuncioService;
            _imovelXMLAppService = imovelXMLAppService;
        }



        public void AtualizarPorImoveis(int[] idImoveis)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorImoveis(idImoveis).OrderBy(_ => _.IdImovel)
                                                                                                 .ToList();
            AtualizarImoveisXMLs(anuncios);
        }

        public void AtualizarPorCotas(int[] idCotas)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorCotas(idCotas).OrderBy(_ => _.IdImovel)
                                                                                             .ToList();
            AtualizarImoveisXMLs(anuncios);
        }

        public void AtualizarPorPortais(Portal[] portais)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorPortais(portais).OrderBy(_ => _.Portal)
                                                                                               .ThenBy(_ => _.IdEmpresa)
                                                                                               .ThenBy(_ => _.IdImovel)
                                                                                               .ToList();
            AtualizarImoveisXMLs(anuncios);
        }

        private void AtualizarImoveisXMLs(IEnumerable<Anuncio> anuncios)
        {
            //TODO: agrupar por portal/empresa e executar em paralelo
            int i = 0;
            int totalAnuncios = anuncios.Count();
            int partitionIds = 0;

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdEmpresa }, (key, group) => new
            {
                Portal = key.Portal,
                IdEmpresa = key.IdEmpresa,
                Anuncios = group.ToList()
            }).ToList();


            // Get the partitioner.
            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                //TODO: Obter de DI
                //_serviceProvider.GetRequiredService<IEmpresaApelidoPortalRepository>();
                //var repo =  new EmpresaApelidoPortalRepository(new DbLopesnetContext());
                //var imovelXMLAppService = new ImovelXMLAppService(@"C:\Temp\portais", repo, _logger);

                using (partition)
                    while (partition.MoveNext())
                    {
                        var portalEmpresa = partition.Current;


                        IRetorno<string> caminhoArquivo = _imovelXMLAppService.CaminhoArquivoXml(portalEmpresa.Portal, portalEmpresa.IdEmpresa);
                        if(!caminhoArquivo.Sucesso)
                        {
                            _logger.Error($"Erro ao obter o caminho do arquivo: {caminhoArquivo.ErrosConcatenados()}.");
                            continue;
                        }
                        IPortalXMLBuilder builder = PortalXMLBuilder.ObterXmlBuilder(portalEmpresa.Portal, caminhoArquivo.Dado);


                        foreach (Anuncio anuncio in portalEmpresa.Anuncios)
                        {
                            i++;
                            if (i % 100 == 0)
                                _logger.Debug($"{i} anúncios processados de {anuncios.Count()}, {(i/totalAnuncios)*100}% completo.");

                            bool imovelNoXml = builder.ImovelNoXml(anuncio.IdImovel);
                            StatusAnuncioPortal statusImovelPortal = _statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoXml);

                            if (statusImovelPortal == StatusAnuncioPortal.Atualizado ||
                                statusImovelPortal == StatusAnuncioPortal.Removido)
                            {
                                _logger.Debug($"{InicioLog(anuncio, i, partitionId)}  já {(statusImovelPortal == StatusAnuncioPortal.Removido ? "removido" : "atualizado")}.");
                                continue;
                            }

                            if (statusImovelPortal == StatusAnuncioPortal.ARemover)
                            {
                                builder.RemoverImovel(anuncio.IdImovel);
                                RegistrarRemocaoImovelPortal(anuncio, AtualizacaoAcao.Exclusao);
                                _logger.Debug($"{InicioLog(anuncio, i, partitionId)}  removido.");
                            }
                            else if (statusImovelPortal == StatusAnuncioPortal.Desatualizado)
                            {
                                Imovel dados = ObterDadosImovel(anuncio.IdImovel);
                                if (dados == null)
                                    _logger.Error(InicioLog(anuncio, i, partitionId) + "Imóvel não encontrado.");

                                dados.CodigoClientePortal = anuncio.CodigoClientePortal;

                                builder.InserirAtualizarImovel(dados);
                                RegistrarRemocaoImovelPortal(anuncio, AtualizacaoAcao.Atualizacao);
                                _logger.Debug($"{InicioLog(anuncio, i, partitionId)}  inserido/atualizado.");
                            }
                        }

                    }
            })).ToArray();
            Task.WaitAll(tasks);
        }

        private static string InicioLog(Anuncio anuncio, int i, int processador)
        {
            return $"P{processador} - {i} Imóvel {anuncio.IdImovel.ToString().PadLeft(6)}  Portal: {anuncio.Portal.ToString().PadRight(10)}  Empresa: {anuncio.NomeEmpresa.PadRight(40)} :: ";
        }
        private void RegistrarRemocaoImovelPortal(Anuncio anuncio, AtualizacaoAcao acao)
        {
            var model = new AnuncioAtualizacao(anuncio.Portal, anuncio.IdImovel, anuncio.IdEmpresa, acao);
            _imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(model);
        }

        private Imovel ObterDadosImovel(int idImovel)
        {
            Imovel? dados = _dadosImoveisCache.FirstOrDefault(_ => _.IdImovel == idImovel);
            if (dados != null)
                return dados;

            dados = _dadosImovelAppService.ObterDadosImovel(idImovel);
            if (dados == null)
                return null;

            _dadosImoveisCache.Add(dados);

            return dados;
        }
    }
}