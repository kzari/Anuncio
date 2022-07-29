using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
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
        private readonly IPortalAtualizadorFactory _portalAtualizadorFactory;

        private readonly List<DadosImovel> _dadosImoveisCache;

        public AtualizarAnunciosAppService(ILogger logger,
                                           IAnuncioAppService anuncioAppService,
                                           IServiceProvider serviceProvider,
                                           IPortalAtualizadorFactory portalAtualizadorFactory)
        {
            _logger = logger;
            _anuncioAppService = anuncioAppService;
            _dadosImoveisCache = new List<DadosImovel>();
            _serviceProvider = serviceProvider;
            _portalAtualizadorFactory = portalAtualizadorFactory;
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
            int totalAnuncios = anuncios.Count();
            int partitionIds = 0;
            int cotaAtual = 0;

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdEmpresa }, (key, group) => new
            {
                Portal = key.Portal,
                IdEmpresa = key.IdEmpresa,
                Anuncios = group.ToList()
            }).ToList();
            int qtdeCotas = anunciosAgrupados.Count();

            IProgresso progressoGeral = _logger.ObterProgresso(anunciosAgrupados.Count(), 110, $">> Atualização de imóveis nos portais.");
            progressoGeral.Atualizar($"Processando {qtdeCotas} cotas.");

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
            //var partitions = partitioner.GetPartitions(1);

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IStatusAnuncioService statusAnuncioService = (IStatusAnuncioService)_serviceProvider.GetService(typeof(IStatusAnuncioService));
                IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository = (IImovelAtualizacaoPortaisRepository)_serviceProvider.GetService(typeof(IImovelAtualizacaoPortaisRepository));

                using (partition)
                    while (partition.MoveNext())
                    {
                        cotaAtual++;
                        var portalEmpresa = partition.Current;

                        int idEmpresa = portalEmpresa.IdEmpresa;
                        Portal portal = portalEmpresa.Portal;
                        List<Anuncio> anuncios = portalEmpresa.Anuncios;
                        int qtdeAnuncios = anuncios.Count;

                        List<int> imoveisParaRemover = new();
                        List<int> imoveisParaAtualizar = new();
                        int jaRemovidos = 0;
                        int jaAtualizados = 0;

                        IProgresso progresso = _logger.ObterProgresso(qtdeAnuncios, 110, textoInicial: $"P{partitionId.ToString().PadLeft(2)} E: {idEmpresa.ToString().PadRight(5)} P: {portal.ToString().PadRight(10)} Anúncios: {qtdeAnuncios.ToString().PadLeft(5)} ");

                        progresso.Atualizar("1. Verificando o status...", percentualConcluido: 10);

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portalEmpresa.Portal, portalEmpresa.IdEmpresa);

                        int[] idImoveisNoPortal = atualizador.ObterIdImoveisNoPortal().ToArray();

                        foreach (Anuncio anuncio in portalEmpresa.Anuncios)
                        {
                            bool imovelNoPortal = idImoveisNoPortal.Contains(anuncio.IdImovel);

                            StatusAnuncioPortal statusImovelPortal = statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoPortal);
                            switch (statusImovelPortal)
                            {
                                case StatusAnuncioPortal.Atualizado:
                                    jaAtualizados++;
                                    continue;

                                case StatusAnuncioPortal.Removido:
                                    jaRemovidos++;
                                    continue;

                                case StatusAnuncioPortal.ARemover:
                                    imoveisParaRemover.Add(anuncio.IdImovel);
                                    break;

                                case StatusAnuncioPortal.Desatualizado:
                                    imoveisParaAtualizar.Add(anuncio.IdImovel);
                                    break;
                            }
                        }

                        progresso.Atualizar($"2. Removendo {imoveisParaRemover.Count} anúncios...", percentualConcluido: 10);
                        Remover(imovelAtualizacaoPortaisRepository, idEmpresa, portal, imoveisParaRemover, atualizador, progresso);

                        //Atualizar(anuncios, imoveisParaAtualizar, idEmpresa, portal, atualizador, imovelAtualizacaoPortaisRepository, dadosImovelAppService, progresso);
                        AtualizarLote(anuncios, imoveisParaAtualizar, idEmpresa, portal, atualizador, imovelAtualizacaoPortaisRepository, progresso);

                        progresso.Atualizar($"Concluído. R: ({imoveisParaRemover.Count} | {jaRemovidos}) A: ({imoveisParaAtualizar.Count} | {jaAtualizados}).", percentualConcluido: 100);

                        progressoGeral.Atualizar($"Processando cotas {cotaAtual} de {qtdeCotas}.");
                    }
            })).ToArray();
            Task.WaitAll(tasks);

            progressoGeral.Atualizar($"Atualização concluída. {qtdeCotas} cotas processadas, {totalAnuncios} anúncios processados.", percentualConcluido: 100);
        }

        private static void Remover(IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository, int idEmpresa, Portal portal, List<int> imoveisParaRemover, IPortalAtualizador atualizador, IProgresso progresso)
        {
            atualizador.RemoverImoveis(imoveisParaRemover.ToArray(), progresso);
            var anuncioAtualizacoes = imoveisParaRemover.Select(_ => new AnuncioAtualizacao(portal, _, idEmpresa, AtualizacaoAcao.Exclusao));
            imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(anuncioAtualizacoes, progresso);
        }

        private void Atualizar(IEnumerable<Anuncio> anuncios,
                               IEnumerable<int> imoveisParaAtualizar,
                               int idEmpresa,
                               Portal portal,
                               IPortalAtualizador atualizador,
                               IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository,
                               IDadosImovelAppService dadosImovelAppService,
                               IProgresso progresso)
        {
            List<DadosImovel> imoveis = new List<DadosImovel>();
            List<AnuncioAtualizacao> atualizacoes = new List<AnuncioAtualizacao>();

            int i = 0;
            foreach (int idImovel in imoveisParaAtualizar)
            {
                i++;
                progresso.Atualizar($"3. Obtendo dados dos imóveis. {i} de {imoveisParaAtualizar.Count()}", i);

                DadosImovel dados = ObterDadosImovel(idImovel, dadosImovelAppService);
                if (dados == null)
                {
                    _logger.Error($"Imóvel não encontrado {idImovel}.");
                }
                else
                {
                    dados.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.IdImovel == idImovel).CodigoClientePortal;
                    imoveis.Add(dados);
                    atualizacoes.Add(new AnuncioAtualizacao(portal, idImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
                }
            }

            if (!imoveisParaAtualizar.Any())
                return;

            progresso.Atualizar($"4. Atualizando status do anúncio/imóvel.", i);

            atualizador.InserirAtualizarImoveis(imoveis, progresso: progresso);
            imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(atualizacoes, progresso);
        }

        private void AtualizarLote(IEnumerable<Anuncio> anuncios,
                               IEnumerable<int> imoveisParaAtualizar,
                               int idEmpresa,
                               Portal portal,
                               IPortalAtualizador atualizador,
                               IImovelAtualizacaoPortaisRepository imovelAtualizacaoPortaisRepository,
                               IProgresso progresso)
        {
            progresso.Atualizar($"3. Obtendo dados dos {imoveisParaAtualizar.Count()} imóveis.", percentualConcluido: 20);

            IDadosImovelAppService dadosImovelAppService = (IDadosImovelAppService)_serviceProvider.GetService(typeof(IDadosImovelAppService));

            IEnumerable<DadosImovel> imoveis = ObterDadosImovelLote(imoveisParaAtualizar.ToArray(), dadosImovelAppService, progresso);

            if (!imoveisParaAtualizar.Any())
                return;

            List<AnuncioAtualizacao> atualizacoes = new List<AnuncioAtualizacao>();
            foreach (DadosImovel imovel in imoveis)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.IdImovel == imovel.Dados.IdImovel).CodigoClientePortal;
                atualizacoes.Add(new AnuncioAtualizacao(portal, imovel.Dados.IdImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Atualizar($"3. Atualizando imóveis...", percentualConcluido: 30);
            atualizador.InserirAtualizarImoveis(imoveis, progresso: progresso);

            progresso.Atualizar($"3. Registrando o status...", percentualConcluido: 30);
            imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(atualizacoes, progresso);
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
            if (dados != null)
            {
                lock (_dadosImoveisCache)
                    _dadosImoveisCache.Add(dados);
            }

            return dados;
        }

        private IEnumerable<DadosImovel> ObterDadosImovelLote(int[] idImoveis, IDadosImovelAppService dadosImovelAppService, IProgresso progresso)
        {
            IEnumerable<DadosImovel> imoveisCacheados;
            lock (_dadosImoveisCache)
            {
                imoveisCacheados = _dadosImoveisCache.ToList().Where(_ => idImoveis.Contains(_.Dados.IdImovel)) ?? new List<DadosImovel>();
            }

            int[] idImoveisNaoCacheados = idImoveis.Where(_ => !imoveisCacheados.Select(_ => _.Dados.IdImovel).Contains(_)).ToArray() ?? idImoveis;

            IEnumerable<DadosImovel> imoveisNaoCacheados = dadosImovelAppService.ObterDadosImovel(idImoveisNaoCacheados, progresso);
            if (imoveisNaoCacheados.Any())
            {
                lock (_dadosImoveisCache)
                    _dadosImoveisCache.AddRange(imoveisNaoCacheados);
            }

            return imoveisCacheados.Concat(imoveisNaoCacheados);
        }
    }
}