using Lopes.Domain.Commons;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using System.Collections.Concurrent;
using Lopes.Infra.Commons;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;

namespace Lopes.Anuncio.Application.Services
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



        public void AtualizarPorImoveis(int[] idImoveis, Portal? portal, ILogger log)
        {
            IEnumerable<AnuncioImovel> anuncios = _anuncioAppService.ObterAnunciosPorImoveis(idImoveis, portal).OrderBy(_ => _.IdImovel)
                                                                                                               .ToList();
            AtualizarImoveisXMLs(anuncios, log);
        }

        public void AtualizarPorCotas(int[] idCotas, ILogger log)
        {
            IEnumerable<AnuncioImovel> anuncios = _anuncioAppService.ObterAnunciosPorCotas(idCotas).OrderBy(_ => _.IdImovel)
                                                                                                   .ToList();
            AtualizarImoveisXMLs(anuncios, log);
        }

        public void AtualizarPorPortais(Portal[] portais, ILogger? log)
        {
            (log ?? _logger).Info($"Buscando anúncios para atualização para os portais: '{string.Join(", ", portais)}'...");

            IEnumerable<AnuncioImovel> anuncios = _anuncioAppService.ObterAnunciosPorPortais(portais).OrderBy(_ => _.Portal)
                                                                                                     .ThenBy(_ => _.IdEmpresa)
                                                                                                     .ThenBy(_ => _.IdImovel)
                                                                                                     .ToList();
            AtualizarImoveisXMLs(anuncios, log);
        }

        private void AtualizarImoveisXMLs(IEnumerable<AnuncioImovel> anuncios, ILogger? log)
        {
            int totalAnuncios = anuncios.Count();

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdEmpresa }, (key, group) => new
            {
                Portal = key.Portal,
                IdEmpresa = key.IdEmpresa,
                Anuncios = group.ToList()
            }).ToList();
            int qtdeCotas = anunciosAgrupados.Count;

            (log ?? _logger).Info($"{totalAnuncios} anúncios encontrados para atualização");

            IProgresso progressoGeral = (log ?? _logger).ObterProgresso(anunciosAgrupados.Count, 95, $">> Atualização de imóveis nos portais.");
            progressoGeral.Atualizar($"Processando {qtdeCotas} cotas.");

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
            //var partitions = partitioner.GetPartitions(1);

            int partitionIds = 0;
            int cotaAtual = 0;

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IStatusAnuncioService statusAnuncioService = (IStatusAnuncioService)_serviceProvider.GetService(typeof(IStatusAnuncioService));
                IAnuncioStatusRepositorioGravacao imovelAtualizacaoPortaisRepository = (IAnuncioStatusRepositorioGravacao)_serviceProvider.GetService(typeof(IAnuncioStatusRepositorioGravacao));

                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        cotaAtual++;
                        var portalEmpresa = partition.Current;

                        int idEmpresa = portalEmpresa.IdEmpresa;
                        Portal portal = portalEmpresa.Portal;
                        List<AnuncioImovel> anuncios = portalEmpresa.Anuncios;
                        int qtdeAnuncios = anuncios.Count;

                        List<int> imoveisParaRemover = new();
                        List<int> imoveisParaAtualizar = new();
                        int jaRemovidos = 0;
                        int jaAtualizados = 0;

                        IProgresso progresso = (log ?? _logger).ObterProgresso(qtdeAnuncios, 95, textoInicial: $"P{partitionId.ToString().PadLeft(2)} E: {idEmpresa.ToString().PadRight(5)} P: {portal.ToString().PadRight(10)} Anúncios: {qtdeAnuncios.ToString().PadLeft(5)} ");

                        progresso.Atualizar("1. Verificando o status...", percentualConcluido: 10);

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idEmpresa);

                        int[] idImoveisNoPortal = atualizador.ObterIdImoveisNoPortal().ToArray();

                        foreach (AnuncioImovel anuncio in anuncios)
                        {
                            bool imovelNoPortal = idImoveisNoPortal.Contains(anuncio.IdImovel);

                            switch (statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoPortal))
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
                        atualizador.RemoverImoveis(imoveisParaRemover.ToArray(), progresso);
                        //List<AnuncioAtualizacao> atualizacoes = imoveisParaRemover.Select(_ => new AnuncioAtualizacao(portal, _, idEmpresa, AtualizacaoAcao.Exclusao)).ToList();
                        List<AtualizarStatusAnuncioRequest> atualizacoes = imoveisParaRemover.Select(_ => new AtualizarStatusAnuncioRequest(portal, _, idEmpresa, AtualizacaoAcao.Exclusao)).ToList();

                        progresso.Atualizar($"3. Atualizando/Adicionando {imoveisParaAtualizar.Count} anúncios...", percentualConcluido: 50);
                        atualizacoes.AddRange(AtualizarAdicionarAnuncios(anuncios, imoveisParaAtualizar, idEmpresa, portal, atualizador, progresso));

                        progresso.Atualizar($"4. Registrando o status...", percentualConcluido: 80);
                        //imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(atualizacoes, progresso);
                        var atualizarStatusAnuncioAppService = _serviceProvider.ObterServico<IAtualizarStatusAnuncioAppService>();
                        IEnumerable<AtualizarStatusAnuncioResponse> response = atualizarStatusAnuncioAppService.Atualizar(atualizacoes);

                        progresso.Atualizar($"Concluído. R: {imoveisParaRemover.Count.ToString().PadLeft(5)} ° {jaRemovidos.ToString().PadLeft(5)} A: {imoveisParaAtualizar.Count.ToString().PadLeft(5)} ° {jaAtualizados.ToString().PadLeft(5)}).", percentualConcluido: 100);

                        progressoGeral.Atualizar($"Processando cotas {cotaAtual} de {qtdeCotas}.");
                    }
                }
            })).ToArray();
            Task.WaitAll(tasks);

            progressoGeral.Atualizar($"Atualização concluída. {qtdeCotas} cotas, {totalAnuncios} anúncios.", percentualConcluido: 100);
        }
        
        private List<AtualizarStatusAnuncioRequest> AtualizarAdicionarAnuncios(IEnumerable<AnuncioImovel> anuncios,
                                                                    IEnumerable<int> imoveisParaAtualizar,
                                                                    int idEmpresa,
                                                                    Portal portal,
                                                                    IPortalAtualizador atualizador,
                                                                    IProgresso progresso)
        {
            if (!imoveisParaAtualizar.Any())
                return new List<AtualizarStatusAnuncioRequest>();

            progresso.Atualizar($"3. Obtendo dados dos {imoveisParaAtualizar.Count()} imóveis.", percentualConcluido: 20);

            IDadosImovelAppService dadosImovelAppService = _serviceProvider.ObterServico<IDadosImovelAppService>();

            IEnumerable<DadosImovel> imoveis = ObterDadosImoveisCache(imoveisParaAtualizar.ToArray(), dadosImovelAppService, progresso);

            //TODO: tratar imóveis não encontrados

            List<AtualizarStatusAnuncioRequest> atualizacoes = new();

            if (!imoveis.Any())
                return atualizacoes;

            foreach (DadosImovel imovel in imoveis)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.IdImovel == imovel.Dados.IdImovel).CodigoClientePortal;
                atualizacoes.Add(new AtualizarStatusAnuncioRequest(portal, imovel.Dados.IdImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Atualizar($"3. Atualizando imóveis...", percentualConcluido: 30);
            atualizador.InserirAtualizarImoveis(imoveis, progresso: progresso);

            return atualizacoes;
        }

        private IEnumerable<DadosImovel> ObterDadosImoveisCache(int[] idImoveis, IDadosImovelAppService dadosImovelAppService, IProgresso progresso)
        {
            IEnumerable<DadosImovel> imoveisCacheados;
            lock (_dadosImoveisCache)
            {
                imoveisCacheados = _dadosImoveisCache.ToList().Where(_ => idImoveis.Contains(_.Dados.IdImovel)) ?? new List<DadosImovel>();
            }

            int[] idImoveisNaoCacheados = idImoveis.Where(_ => !imoveisCacheados.Select(_ => _.Dados.IdImovel).Contains(_)).ToArray() ?? idImoveis;
            if (idImoveisNaoCacheados.Any())
            {
                IEnumerable<DadosImovel> imoveisNaoCacheados = dadosImovelAppService.ObterDadosImovel(idImoveisNaoCacheados, progresso);
                if (imoveisNaoCacheados.Any())
                {
                    lock (_dadosImoveisCache)
                        _dadosImoveisCache.AddRange(imoveisNaoCacheados);
                    return imoveisCacheados.Concat(imoveisNaoCacheados);
                }
            }

            return imoveisCacheados;
        }
    }
}