using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;
using MediatR;
using System.Collections.Concurrent;
using Lopes.Anuncio.Domain.Entidades;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Domain.Commons.Cache;

namespace Lopes.Anuncio.Domain.Handlers
{
    /// <summary>
    /// Faz a atualização dos anúncios nos portais
    /// </summary>
    public class AtualizacaoCommandHandler : IRequestHandler<AnunciosAtualizacaoCommand, bool>
    {
        private const string CHAVE_CACHE_DADOS_IMOVEIS = "DadosProdutos";
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPortalAtualizadorFactory _portalAtualizadorFactory;
        private readonly List<Produto> _dadosProdutosCache;
        private readonly ICacheService _cacheService;

        public AtualizacaoCommandHandler(ILogger logger,
                                         IServiceProvider serviceProvider,
                                         IPortalAtualizadorFactory portalAtualizadorFactory,
                                         ICacheService cacheService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _portalAtualizadorFactory = portalAtualizadorFactory;
            _dadosProdutosCache = new List<Produto>();
            _cacheService = cacheService;
        }


        public Task<bool> Handle(AnunciosAtualizacaoCommand request, CancellationToken cancellationToken)
        {
            _cacheService.Gravar("aa", "AAA", TimeSpan.FromSeconds(5));
            _cacheService.Gravar("bb", "BBB", TimeSpan.FromSeconds(5));

            var aa = _cacheService.Obter<string>("aa");
            var bb = _cacheService.Obter<string>("bb");

            IEnumerable<AnuncioCota> anuncios = request.Anuncios;
            ILogger logger = request.Logger ?? _logger;

            int totalAnuncios = anuncios.Count();

            var anunciosAgrupados = anuncios.GroupBy(_ => new { _.Portal, _.IdFranquia }, (key, group) => new
            {
                Portal = key.Portal,
                IdFranquia = key.IdFranquia,
                Anuncios = group.ToList()
            }).ToList();
            int qtdeCotas = anunciosAgrupados.Count;

            logger.Info($"{totalAnuncios} anúncios encontrados para atualização. {qtdeCotas} cota(s).");

            IProgresso progressoGeral = logger.ObterProgresso(anunciosAgrupados.Count, 95, $">> Atualização de imóveis nos portais.");
            progressoGeral.NovaMensagem($"Processando {qtdeCotas} cota(s).");

            var partitioner = Partitioner.Create(anunciosAgrupados);
            var partitions = partitioner.GetPartitions(Environment.ProcessorCount);
            //var partitions = partitioner.GetPartitions(1);

            int partitionIds = 0;
            int cotaAtual = 0;

            Task[] tasks = partitions.Select(partition => Task.Run(() =>
            {
                int partitionId = partitionIds++;

                IStatusAnuncioService statusAnuncioService = (IStatusAnuncioService)_serviceProvider.GetService(typeof(IStatusAnuncioService));
                IAnuncioStatusRepositorio imovelAtualizacaoPortaisDadosService = (IAnuncioStatusRepositorio)_serviceProvider.GetService(typeof(IAnuncioStatusRepositorio));

                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        cotaAtual++;
                        var portalEmpresa = partition.Current;

                        int idEmpresa = portalEmpresa.IdFranquia;
                        Portal portal = portalEmpresa.Portal;
                        List<AnuncioCota> anuncios = portalEmpresa.Anuncios;
                        int qtdeAnuncios = anuncios.Count;
                        List<int> imoveisParaRemover = new();
                        List<int> imoveisParaAtualizar = new();
                        int jaRemovidos = 0;
                        int jaAtualizados = 0;

                        IProgresso progresso = logger.ObterProgresso(qtdeAnuncios, 95, textoInicial: $"P{partitionId.ToString().PadLeft(2)} E: {idEmpresa.ToString().PadRight(5)} P: {portal.ToString().PadRight(10)} Anúncios: {qtdeAnuncios.ToString().PadLeft(5)} ");

                        progresso.Mensagem("1. Verificando o status...", percentualConcluido: 10);

                        IPortalAtualizador atualizador = _portalAtualizadorFactory.ObterAtualizador(portal, idEmpresa);

                        int[] idProdutosNoPortal = atualizador.ObterIdProdutosNoPortal().ToArray();

                        foreach (AnuncioCota anuncio in anuncios)
                        {
                            bool imovelNoPortal = idProdutosNoPortal.Contains(anuncio.IdProduto);

                            switch (statusAnuncioService.VerificarStatusImovelPortal(anuncio, imovelNoPortal))
                            {
                                case StatusAnuncioPortal.Atualizado:
                                    jaAtualizados++;
                                    continue;

                                case StatusAnuncioPortal.Removido:
                                    jaRemovidos++;
                                    continue;

                                case StatusAnuncioPortal.ARemover:
                                    imoveisParaRemover.Add(anuncio.IdProduto);
                                    break;

                                case StatusAnuncioPortal.Desatualizado:
                                    imoveisParaAtualizar.Add(anuncio.IdProduto);
                                    break;
                            }
                        }

                        progresso.Mensagem($"2. Removendo {imoveisParaRemover.Count} anúncios...", percentualConcluido: 10);
                        atualizador.RemoverProdutos(imoveisParaRemover.ToArray(), progresso);
                        List<AnuncioAtualizacao> atualizacoes = imoveisParaRemover.Select(_ => new AnuncioAtualizacao(portal, _, idEmpresa, AtualizacaoAcao.Exclusao)).ToList();

                        progresso.Mensagem($"3. Atualizando/Adicionando {imoveisParaAtualizar.Count} anúncios...", percentualConcluido: 50);
                        atualizacoes.AddRange(AtualizarAdicionarAnuncios(anuncios, imoveisParaAtualizar, idEmpresa, portal, atualizador, progresso));

                        progresso.Mensagem($"4. Registrando o status...", percentualConcluido: 80);
                        RegistrarAtualizacao(atualizacoes, progresso, cancellationToken);

                        progresso.Mensagem($"Concluído. Removidos: {imoveisParaRemover.Count.ToString().PadLeft(5)} Atualizados/Inseridos: {imoveisParaAtualizar.Count.ToString().PadLeft(5)}.", percentualConcluido: 100);

                        progressoGeral.NovaMensagem($"Processando cotas {cotaAtual} de {qtdeCotas}.");
                    }
                }
            })).ToArray();
            Task.WaitAll(tasks, cancellationToken);

            progressoGeral.Mensagem($"Atualização concluída. {qtdeCotas} cotas, {totalAnuncios} anúncios.", percentualConcluido: 100);


            return Task.FromResult(true);
        }

        private bool RegistrarAtualizacao(List<AnuncioAtualizacao> atualizacoes, IProgresso progresso, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.ObterServico<IRequestHandler<RegistroAtualizacoesCommand, bool>>();
            return handler.Handle(new RegistroAtualizacoesCommand(atualizacoes, progresso), cancellationToken).Result;
        }

        private List<AnuncioAtualizacao> AtualizarAdicionarAnuncios(IEnumerable<AnuncioCota> anuncios,
                                                                               IEnumerable<int> imoveisParaAtualizar,
                                                                               int idEmpresa,
                                                                               Portal portal,
                                                                               IPortalAtualizador atualizador,
                                                                               IProgresso progresso)
        {
            if (!imoveisParaAtualizar.Any())
                return new List<AnuncioAtualizacao>();

            progresso.Mensagem($"3. Obtendo dados dos {imoveisParaAtualizar.Count()} imóveis.", percentualConcluido: 20);

            IEnumerable<Produto> imoveis = ObterDadosProdutosCache(imoveisParaAtualizar.ToArray(), progresso).ToList();

            //TODO: tratar imóveis não encontrados

            List<AnuncioAtualizacao> atualizacoes = new();

            if (!imoveis.Any())
                return atualizacoes;

            foreach (Produto imovel in imoveis)
            {
                imovel.CodigoClientePortal = anuncios.FirstOrDefault(_ => _.IdProduto == imovel.Dados.IdProduto).CodigoClientePortal;
                atualizacoes.Add(new AnuncioAtualizacao(portal, imovel.Dados.IdProduto, idEmpresa, AtualizacaoAcao.Atualizacao));
            }

            progresso.Mensagem($"3. Atualizando imóveis...", percentualConcluido: 30);
            atualizador.InserirAtualizarProdutos(imoveis, progresso: progresso);

            return atualizacoes;
        }


        private IEnumerable<Produto> ObterDadosProdutosCache(int[] idProdutos, IProgresso progresso)
        {
            IEnumerable<Produto>? todosProdutosCacheados = _cacheService.Obter<IEnumerable<Produto>>(CHAVE_CACHE_DADOS_IMOVEIS);

            IEnumerable<Produto> imoveisCacheados = todosProdutosCacheados?.Where(_ => idProdutos.Contains(_.Dados.IdProduto)).ToList() ?? new List<Produto>();

            int[] idProdutosNaoCacheados = idProdutos.Where(_ => !imoveisCacheados.Select(_ => _.Dados.IdProduto).Contains(_)).ToArray() ?? idProdutos;
            if (idProdutosNaoCacheados.Any())
            {
                IEnumerable<Produto> imoveisNaoCacheados = ObterDadosImovel(idProdutosNaoCacheados, progresso).ToList();
                if (imoveisNaoCacheados.Any())
                {
                    _cacheService.Gravar(CHAVE_CACHE_DADOS_IMOVEIS, imoveisNaoCacheados, TimeSpan.FromHours(1));
                    return imoveisCacheados.Concat(imoveisNaoCacheados);
                }
            }

            return imoveisCacheados;
        }

        private IEnumerable<Produto> ObterDadosProdutosCacheOLD(int[] idProdutos, IProgresso progresso)
        {
            IEnumerable<Produto> imoveisCacheados;
            lock (_dadosProdutosCache)
            {
                imoveisCacheados = _dadosProdutosCache.ToList().Where(_ => idProdutos.Contains(_.Dados.IdProduto)) ?? new List<Produto>();
            }

            int[] idProdutosNaoCacheados = idProdutos.Where(_ => !imoveisCacheados.Select(_ => _.Dados.IdProduto).Contains(_)).ToArray() ?? idProdutos;
            if (idProdutosNaoCacheados.Any())
            {
                IEnumerable<Produto> imoveisNaoCacheados = ObterDadosImovel(idProdutosNaoCacheados, progresso);
                if (imoveisNaoCacheados.Any())
                {
                    lock (_dadosProdutosCache)
                        _dadosProdutosCache.AddRange(imoveisNaoCacheados);
                    return imoveisCacheados.Concat(imoveisNaoCacheados);
                }
            }

            return imoveisCacheados;
        }

        public IEnumerable<Produto> ObterDadosImovel(int[] idProdutos, IProgresso progresso = null)
        {
            if (progresso != null)
                progresso.NovaMensagem($"Obtendo dados principais de {idProdutos.Length} imóveis.");

            IProdutoDadosService imovelDadosService = _serviceProvider.ObterServico<IProdutoDadosService>();

            IEnumerable<DadosPrincipais> dadosPrincipais = imovelDadosService.ObterDados(idProdutos);

            int[] idProdutosResgatados = dadosPrincipais.Select(_ => _.IdProduto).ToArray();

            if (progresso != null)
                progresso.NovaMensagem($"Obtendo caracteristicas de {idProdutosResgatados.Length} imóveis.");

            List<Caracteristica> caracteristicas = imovelDadosService.ObterCaracteristicas(idProdutosResgatados).ToList();

            if (progresso != null)
                progresso.NovaMensagem($"Obtendo Tour virtual e Vídeos de {idProdutosResgatados.Count()} imóveis.");

            IDictionary<int, string[]> urlTours = imovelDadosService.ObterUrlTourVirtuais(idProdutosResgatados);
            IDictionary<int, string[]> urlVideos = imovelDadosService.ObterUrlVideos(idProdutosResgatados);
            IEnumerable<Foto> fotos = imovelDadosService.ObterFotos(idProdutosResgatados);

            if (progresso != null)
                progresso.NovaMensagem($"Preenchendo informações de {idProdutosResgatados.Length} imóveis.");

            List<Produto> imoveis = new List<Produto>();
            foreach (DadosPrincipais dados in dadosPrincipais)
            {
                Produto imovel = new(dados);

                imovel.Caracteristicas = caracteristicas.Where(_ => _.IdImovel == dados.IdProduto);
                imovel.UrlTourVirtuais = urlTours.Where(_ => _.Key == dados.IdProduto).SelectMany(_ => _.Value);
                imovel.UrlVideos = urlVideos.Where(_ => _.Key == dados.IdProduto).SelectMany(_ => _.Value);
                imovel.Imagens = fotos.Where(_ => _.IdProduto == dados.IdProduto);

                yield return imovel;
            }
        }
    }
}
