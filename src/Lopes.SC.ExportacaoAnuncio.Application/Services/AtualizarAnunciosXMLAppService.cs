using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.ExportacaoAnuncio.Domain.XML;
using System.Collections.Concurrent;


namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{
    public class AtualizarAnunciosXMLAppService : IAtualizarAnunciosAppService
    {
        private readonly ILogger _logger;
        private readonly IAnuncioAppService _anuncioAppService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPortalXMLBuilder _portalXMLBuilder;

        private readonly List<DadosImovel> _dadosImoveisCache;

        public AtualizarAnunciosXMLAppService(ILogger logger,
                                              IAnuncioAppService anuncioAppService,
                                              IServiceProvider serviceProvider,
                                              IPortalXMLBuilder portalXMLBuilder)
        {
            _logger = logger;
            _anuncioAppService = anuncioAppService;
            _dadosImoveisCache = new List<DadosImovel>();
            _serviceProvider = serviceProvider;
            _portalXMLBuilder = portalXMLBuilder;
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
            //var partitions = partitioner.GetPartitions(1);

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

                        IPortalXMLElementos portalElementos = PortalXmlElementosBase.ObterPortalXml(portalEmpresa.Portal);


                        IRetorno<string> retornoCaminhoArquivo = imovelXMLAppService.CaminhoArquivoXml(portalEmpresa.Portal, portalEmpresa.IdEmpresa);
                        if(!retornoCaminhoArquivo.Sucesso)
                        {
                            _logger.Error($"Erro ao obter o caminho do arquivo: {retornoCaminhoArquivo.ErrosConcatenados()}.");
                            continue;
                        }
                        //IPortalXMLBuilder builder = PortalXMLBuilder.ObterXmlBuilder(portalEmpresa.Portal, caminhoArquivo.Dado);

                        List<DadosImovel> imoveisParaAtualizar = new List<DadosImovel>();

                        foreach (Anuncio anuncio in portalEmpresa.Anuncios)
                        {
                            //i++;
                            //if (i % 1000 == 0)
                            //    _logger.Debug($"-- {i} anúncios processados de {anuncios.Count()}, {(i/totalAnuncios)*100}% completo.");

                            bool imovelNoXml = _portalXMLBuilder.ImovelNoXml(anuncio.IdImovel, retornoCaminhoArquivo.Dado);
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
                                _portalXMLBuilder.RemoverImovel(anuncio.IdImovel, retornoCaminhoArquivo.Dado);

                                imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(new AnuncioAtualizacao(anuncio.Portal,
                                                                                                               anuncio.IdImovel,
                                                                                                               anuncio.IdEmpresa,
                                                                                                               AtualizacaoAcao.Exclusao));
                                //_logger.Debug($"{InicioLog(anuncio, i, partitionId)}  removido.");
                                removidos++;
                            }
                            else if (statusImovelPortal == StatusAnuncioPortal.Desatualizado)
                            {
                                DadosImovel dados = ObterDadosImovel(anuncio.IdImovel, dadosImovelAppService);
                                if (dados == null)
                                    _logger.Error(InicioLog(anuncio, i, partitionId) + $"Imóvel não encontrado {anuncio.IdImovel}.");
                                else
                                {
                                    dados.CodigoClientePortal = anuncio.CodigoClientePortal;

                                    imoveisParaAtualizar.Add(dados);
                                }
                            }
                        }

                        Atualizar(imoveisParaAtualizar, portalEmpresa.IdEmpresa, portalEmpresa.Portal, retornoCaminhoArquivo.Dado, portalElementos, imovelAtualizacaoPortaisRepository);
                        

                        string anuncios = $"Total anúncios: {portalEmpresa.Anuncios.Count().ToString().PadLeft(5)} " +
                                          $"Atualizados: {imoveisParaAtualizar.Count().ToString().PadLeft(5)} " +
                                          $"Removidos: {removidos.ToString().PadLeft(5)} " +
                                          $"Já atualizados: {jaAtualizados.ToString().PadLeft(5)} " +
                                          $"Já removidos: {jaRemovidos.ToString().PadLeft(5)} ";
                        _logger.Info($"P{partitionId} - Portal: {portalEmpresa.Portal.ToString().PadRight(10)}  Empresa: {portalEmpresa.IdEmpresa.ToString().PadLeft(3)} {anuncios}  Arquivo: {retornoCaminhoArquivo.Dado}");
                    }
            })).ToArray();
            Task.WaitAll(tasks);
        }


        private void Atualizar(List<DadosImovel> imoveisParaAtualizar, 
                               int idEmpresa, 
                               Portal portal, 
                               string caminhoArquivo, 
                               IPortalXMLElementos portalElementos, 
                               IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository)
        {
            if (!imoveisParaAtualizar.Any())
                return;

            Xml xml = portalElementos.ObterXml(imoveisParaAtualizar);
            _portalXMLBuilder.InserirAtualizarImoveis(xml, caminhoArquivo);

            foreach (int idImovel in imoveisParaAtualizar.Select(_ => _.Dados.IdImovel))
            {
                imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(new AnuncioAtualizacao(portal, idImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
            }
        }

        private static string InicioLog(Anuncio anuncio, int i, int processador)
        {
            return $"P{processador} - {i} Imóvel {anuncio.IdImovel.ToString().PadLeft(6)}  Portal: {anuncio.Portal.ToString().PadRight(10)}  Empresa: {anuncio.NomeEmpresa.PadRight(40)} :: ";
        }

        private DadosImovel? ObterDadosImovel(int idImovel, IDadosImovelAppService dadosImovelAppService)
        {
            DadosImovel? dados;
            lock (_dadosImoveisCache)
            { 
                dados = _dadosImoveisCache.ToList().FirstOrDefault(_ => _.Dados.IdImovel == idImovel);
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