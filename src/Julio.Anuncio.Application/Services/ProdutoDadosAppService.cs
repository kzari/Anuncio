using Julio.Anuncio.Application.Interfaces.DadosService;
using Julio.Anuncio.Domain.Models;
using Julio.Anuncio.Domain.Models.DadosProduto;
using Julio.Anuncio.Domain.Services;
using Julio.Domain.Commons;
using Julio.Domain.Commons.Cache;

namespace Julio.Anuncio.Application.Services
{
    public class ProdutoDadosAppService : IProdutoService
    {
        private const string CHAVE_CACHE_DADOS_PRODUTOS = "DadosProdutos";
        private readonly IProdutoDadosService _produtoDadosService;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        public ProdutoDadosAppService(IProdutoDadosService produtoDadosService, 
                                      ICacheService cacheService, 
                                      ILogger logger)
        {
            _produtoDadosService = produtoDadosService;
            _cacheService = cacheService;
            _logger = logger;
        }


        public IEnumerable<Produto> ObterDados(int[] idProdutos, IProgresso? progresso = null)
        {
            IEnumerable<Produto>? todosProdutosCacheados = _cacheService.Obter<IEnumerable<Produto>>(CHAVE_CACHE_DADOS_PRODUTOS);

            IEnumerable<Produto> produtosCacheados = todosProdutosCacheados?.Where(_ => idProdutos.Contains(_.Dados.IdProduto)).ToList() ?? new List<Produto>();

            int[] idProdutosNaoCacheados = idProdutos.Where(_ => !produtosCacheados.Select(_ => _.Dados.IdProduto).Contains(_)).ToArray() ?? idProdutos;
            if (idProdutosNaoCacheados.Length > 0)
            {
                progresso?.NovaMensagem($"Obtendo dados principais de {idProdutos.Length} produtos.");

                IEnumerable<Produto> produtosNaoCacheados = ObterDadosDaBase(idProdutos, progresso).ToList();
                if (produtosNaoCacheados.Any())
                {
                    IEnumerable<Produto> produtos = produtosCacheados.Concat(produtosNaoCacheados);
                    _cacheService.Gravar(CHAVE_CACHE_DADOS_PRODUTOS, produtos, TimeSpan.FromHours(1));
                    return produtos;
                }
            }

            return produtosCacheados;
        }

        public int[] ObterFranquias(int idProduto)
        {
            return _produtoDadosService.ObterFranquias(idProduto);
        }


        private IEnumerable<Produto> ObterDadosDaBase(int[] idProdutos, IProgresso? progresso = null)
        {
            IEnumerable<DadosPrincipais> dadosPrincipais = _produtoDadosService.ObterDados(idProdutos);

            int[] idProdutosEncontrados = dadosPrincipais.Select(_ => _.IdProduto).ToArray();
            int[] idProdutosNaoEncontrados = idProdutos.Where(_ => !idProdutosEncontrados.Contains(_)).ToArray();

            if(idProdutosNaoEncontrados.Length > 0)
                _logger.Warn($"Imóveis não encontrados: {string.Join(", ", idProdutosNaoEncontrados)}");

            progresso?.NovaMensagem($"Obtendo caracteristicas de {idProdutosEncontrados.Length} imóveis.");

            List<Caracteristica> caracteristicas = _produtoDadosService.ObterCaracteristicas(idProdutosEncontrados).ToList();

            progresso?.NovaMensagem($"Obtendo Tour virtual e Vídeos de {idProdutosEncontrados.Length} imóveis.");

            IDictionary<int, string[]> urlTours = _produtoDadosService.ObterUrlTourVirtuais(idProdutosEncontrados);
            IDictionary<int, string[]> urlVideos = _produtoDadosService.ObterUrlVideos(idProdutosEncontrados);
            IEnumerable<Foto> fotos = _produtoDadosService.ObterFotos(idProdutosEncontrados);

            progresso?.NovaMensagem($"Preenchendo informações de {idProdutosEncontrados.Length} imóveis.");

            List<Produto> produtos = new List<Produto>();
            foreach (DadosPrincipais dados in dadosPrincipais)
            {
                Produto imovel = new(dados);

                imovel.Caracteristicas = caracteristicas.Where(_ => _.IdProduto == dados.IdProduto);
                imovel.UrlTourVirtuais = urlTours.Where(_ => _.Key == dados.IdProduto).SelectMany(_ => _.Value);
                imovel.UrlVideos = urlVideos.Where(_ => _.Key == dados.IdProduto).SelectMany(_ => _.Value);
                imovel.Fotos = fotos.Where(_ => _.IdProduto == dados.IdProduto);

                yield return imovel;
            }
        }
    }
}
