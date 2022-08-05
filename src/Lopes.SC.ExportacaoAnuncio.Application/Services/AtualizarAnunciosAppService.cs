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



        public void AtualizarPorImoveis(int[] idImoveis, Portal? portal, ILogger log)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorImoveis(idImoveis, portal).OrderBy(_ => _.IdImovel)
                                                                                                 .ToList();
            AtualizarImoveisXMLs(anuncios, log);
        }

        public void AtualizarPorCotas(int[] idCotas, ILogger log)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorCotas(idCotas).OrderBy(_ => _.IdImovel)
                                                                                             .ToList();
            AtualizarImoveisXMLs(anuncios, log);
        }

        public void AtualizarPorPortais(Portal[] portais, ILogger log)
        {
            IEnumerable<Anuncio> anuncios = _anuncioAppService.ObterAnunciosPorPortais(portais).OrderBy(_ => _.Portal)
                                                                                               .ThenBy(_ => _.IdEmpresa)
                                                                                               .ThenBy(_ => _.IdImovel)
                                                                                               .ToList();
            AtualizarImoveisXMLs(anuncios, log);
        }

        private void AtualizarImoveisXMLs(IEnumerable<Anuncio> anuncios, ILogger log)
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

            (log ?? _logger).Info($"{totalAnuncios} anúncios encontrados para atualização");

            IProgresso progressoGeral = (log ?? _logger).ObterProgresso(anunciosAgrupados.Count(), 95, $">> Atualização de imóveis nos portais.");
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

                        IProgresso progresso = (log ?? _logger).ObterProgresso(qtdeAnuncios, 95, textoInicial: $"P{partitionId.ToString().PadLeft(2)} E: {idEmpresa.ToString().PadRight(5)} P: {portal.ToString().PadRight(10)} Anúncios: {qtdeAnuncios.ToString().PadLeft(5)} ");

                        progresso.Atualizar("1. Verificando o status...", percentualConcluido: 10);

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idEmpresa);

                        int[] idImoveisNoPortal = atualizador.ObterIdImoveisNoPortal().ToArray();

                        foreach (Anuncio anuncio in anuncios)
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
                        atualizador.RemoverImoveis(imoveisParaRemover.ToArray(), progresso);


                        List<AnuncioAtualizacao> atualizacoes = imoveisParaRemover.Select(_ => new AnuncioAtualizacao(portal, _, idEmpresa, AtualizacaoAcao.Exclusao)).ToList();
                        atualizacoes.AddRange(Atualizar(anuncios, imoveisParaAtualizar, idEmpresa, portal, atualizador, progresso));
                        
                        
                        progresso.Atualizar($"3. Registrando o status...", percentualConcluido: 30);
                        imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(atualizacoes, progresso);


                        progresso.Atualizar($"Concluído. R: {imoveisParaRemover.Count.ToString().PadLeft(5)} ° {jaRemovidos.ToString().PadLeft(5)} A: {imoveisParaAtualizar.Count.ToString().PadLeft(5)} ° {jaAtualizados.ToString().PadLeft(5)}).", percentualConcluido: 100);


                        imovelAtualizacaoPortaisRepository.AtualizarOuAdicionar(atualizacoes, progresso);

                        progressoGeral.Atualizar($"Processando cotas {cotaAtual} de {qtdeCotas}.");
                    }
            })).ToArray();
            Task.WaitAll(tasks);

            progressoGeral.Atualizar($"Atualização concluída. {qtdeCotas} cotas, {totalAnuncios} anúncios.", percentualConcluido: 100);
        }
        
        private List<AnuncioAtualizacao> Atualizar(IEnumerable<Anuncio> anuncios,
                                                   IEnumerable<int> imoveisParaAtualizar,
                                                   int idEmpresa,
                                                   Portal portal,
                                                   IPortalAtualizador atualizador,
                                                   IProgresso progresso)
        {
            progresso.Atualizar($"3. Obtendo dados dos {imoveisParaAtualizar.Count()} imóveis.", percentualConcluido: 20);

            IDadosImovelAppService dadosImovelAppService = (IDadosImovelAppService)_serviceProvider.GetService(typeof(IDadosImovelAppService));

            IEnumerable<DadosImovel> imoveis = ObterDadosImoveis(imoveisParaAtualizar.ToArray(), dadosImovelAppService, progresso);

            //TODO: tratar imóveis não encontrados

            List<AnuncioAtualizacao> atualizacoes = new List<AnuncioAtualizacao>();

            if (!imoveis.Any())
                return atualizacoes;

            foreach (DadosImovel imovel in imoveis)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.IdImovel == imovel.Dados.IdImovel).CodigoClientePortal;
                atualizacoes.Add(new AnuncioAtualizacao(portal, imovel.Dados.IdImovel, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Atualizar($"3. Atualizando imóveis...", percentualConcluido: 30);
            atualizador.InserirAtualizarImoveis(imoveis, progresso: progresso);

            return atualizacoes;
        }

        private IEnumerable<DadosImovel> ObterDadosImoveis(int[] idImoveis, IDadosImovelAppService dadosImovelAppService, IProgresso progresso)
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