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
        private readonly IAnuncioAppService _anuncioAppService;
        private readonly IServiceProvider _serviceProvider;

        private readonly List<Imovel> _dadosImoveisCache;

        public AtualizarAnunciosAppService(ILogger logger,
                                           IAnuncioAppService anuncioAppService,
                                           IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository,
                                           IServiceProvider serviceProvider)
        {
            _logger = logger;
            _anuncioAppService = anuncioAppService;
            _dadosImoveisCache = new List<Imovel>();
            _serviceProvider = serviceProvider;
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
            int i = 0;
            int totalAnuncios = anuncios.Count();
            int partitionIds = 0;

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdEmpresa }, (key, group) => new
            {
                Portal = key.Portal,
                IdEmpresa = key.IdEmpresa,
                Anuncios = group.ToList()
            }).ToList();

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IImovelXMLAppService imovelXMLAppService = (IImovelXMLAppService)_serviceProvider.GetService(typeof(IImovelXMLAppService));
                IStatusAnuncioService statusAnuncioService = (IStatusAnuncioService)_serviceProvider.GetService(typeof(IStatusAnuncioService));
                IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository = (IImovelAtualizacaoPortaisRepository)_serviceProvider.GetService(typeof(IImovelAtualizacaoPortaisRepository));
                IDadosImovelAppService dadosImovelAppService = (IDadosImovelAppService)_serviceProvider.GetService(typeof(IDadosImovelAppService));

                using (partition)
                    while (partition.MoveNext())
                    {
                        var portalEmpresa = partition.Current;
                        int removidos = 0;
                        int jaRemovidos = 0;
                        int atualizados = 0;
                        int jaAtualizados  = 0;

                        IRetorno<string> caminhoArquivo = imovelXMLAppService.CaminhoArquivoXml(portalEmpresa.Portal, portalEmpresa.IdEmpresa);
                        if(!caminhoArquivo.Sucesso)
                        {
                            _logger.Error($"Erro ao obter o caminho do arquivo: {caminhoArquivo.ErrosConcatenados()}.");
                            continue;
                        }
                        IPortalXMLBuilder builder = PortalXMLBuilder.ObterXmlBuilder(portalEmpresa.Portal, caminhoArquivo.Dado);


                        foreach (Anuncio anuncio in portalEmpresa.Anuncios)
                        {
                            //i++;
                            //if (i % 1000 == 0)
                            //    _logger.Debug($"-- {i} anúncios processados de {anuncios.Count()}, {(i/totalAnuncios)*100}% completo.");

                            bool imovelNoXml = builder.ImovelNoXml(anuncio.IdImovel);
                            StatusAnuncioPortal statusImovelPortal = statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoXml);

                            if (statusImovelPortal == StatusAnuncioPortal.Atualizado)
                            {
                                jaAtualizados++;
                                continue;
                            }
                            if (statusImovelPortal == StatusAnuncioPortal.Removido)
                            {
                                jaRemovidos++;
                                continue;
                            }

                            if (statusImovelPortal == StatusAnuncioPortal.ARemover)
                            {
                                builder.RemoverImovel(anuncio.IdImovel);

                                imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(new AnuncioAtualizacao(anuncio.Portal,
                                                                                                               anuncio.IdImovel,
                                                                                                               anuncio.IdEmpresa,
                                                                                                               AtualizacaoAcao.Exclusao));
                                //_logger.Debug($"{InicioLog(anuncio, i, partitionId)}  removido.");
                                removidos++;
                            }
                            else if (statusImovelPortal == StatusAnuncioPortal.Desatualizado)
                            {
                                Imovel dados = ObterDadosImovel(anuncio.IdImovel, dadosImovelAppService);
                                if (dados == null)
                                    _logger.Error(InicioLog(anuncio, i, partitionId) + "Imóvel não encontrado.");
                                else
                                {
                                    dados.CodigoClientePortal = anuncio.CodigoClientePortal;

                                    builder.InserirAtualizarImovel(dados);

                                    imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(new AnuncioAtualizacao(anuncio.Portal,
                                                                                                                   anuncio.IdImovel,
                                                                                                                   anuncio.IdEmpresa,
                                                                                                                   AtualizacaoAcao.Atualizacao));
                                    //_logger.Debug($"{InicioLog(anuncio, i, partitionId)}  inserido/atualizado.");
                                    atualizados++;
                                }
                            }
                        }


                        string anuncios = $"Total anúncios: {portalEmpresa.Anuncios.Count().ToString().PadLeft(5)} " +
                                          $"Atualizados: {atualizados.ToString().PadLeft(5)} " +
                                          $"Removidos: {removidos.ToString().PadLeft(5)} " +
                                          $"Já atualizados: {jaAtualizados.ToString().PadLeft(5)} " +
                                          $"Já removidos: {jaRemovidos.ToString().PadLeft(5)} ";
                        _logger.Info($"-- Portal: {portalEmpresa.Portal.ToString().PadRight(10)}  Empresa: {portalEmpresa.IdEmpresa.ToString().PadLeft(3)} {anuncios}  Arquivo: {caminhoArquivo.Dado}");
                    }
            })).ToArray();
            Task.WaitAll(tasks);
        }

        private static string InicioLog(Anuncio anuncio, int i, int processador)
        {
            return $"P{processador} - {i} Imóvel {anuncio.IdImovel.ToString().PadLeft(6)}  Portal: {anuncio.Portal.ToString().PadRight(10)}  Empresa: {anuncio.NomeEmpresa.PadRight(40)} :: ";
        }

        private Imovel ObterDadosImovel(int idImovel, IDadosImovelAppService dadosImovelAppService)
        {
            Imovel? dados;
            lock (_dadosImoveisCache)
            { 
                dados = _dadosImoveisCache.ToList().FirstOrDefault(_ => _.IdImovel == idImovel);
                if (dados != null)
                    return dados;
            }

            dados = dadosImovelAppService.ObterDadosImovel(idImovel);
            if (dados == null)
                return null;

            lock (_dadosImoveisCache)
                _dadosImoveisCache.Add(dados);

            return dados;
        }
    }
}