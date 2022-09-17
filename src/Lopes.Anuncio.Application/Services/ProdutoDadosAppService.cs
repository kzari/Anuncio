using Lopes.Anuncio.Application.Interfaces.DadosService;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;
using Lopes.Domain.Commons.Cache;

namespace Lopes.Anuncio.Application.Services
{
    public class ProdutoDadosAppService : IProdutoService
    {
        private const string CHAVE_CACHE_DADOS_PRODUTOS = "DadosProdutos";
        private readonly IProdutoDadosService _produtoDadosService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICacheService _cacheService;

        public ProdutoDadosAppService(IProdutoDadosService produtoDadosService, 
                                      ICacheService cacheService, 
                                      IServiceProvider serviceProvider)
        {
            _produtoDadosService = produtoDadosService;
            _cacheService = cacheService;
            _serviceProvider = serviceProvider;
        }


        public IEnumerable<Produto> ObterDados(int[] idProdutos, IProgresso? progresso = null)
        {
            IEnumerable<Produto>? todosProdutosCacheados = _cacheService.Obter<IEnumerable<Produto>>(CHAVE_CACHE_DADOS_PRODUTOS);

            IEnumerable<Produto> produtosCacheados = todosProdutosCacheados?.Where(_ => idProdutos.Contains(_.Dados.IdProduto)).ToList() ?? new List<Produto>();

            int[] idProdutosNaoCacheados = idProdutos.Where(_ => !produtosCacheados.Select(_ => _.Dados.IdProduto).Contains(_)).ToArray() ?? idProdutos;
            if (idProdutosNaoCacheados.Any())
            {
                if (progresso != null)
                    progresso.NovaMensagem($"Obtendo dados principais de {idProdutos.Length} produtos.");

                IEnumerable<Produto> produtosNaoCacheados = ObterDadosDaBase(idProdutos, progresso);
                if (produtosNaoCacheados.Any())
                {
                    _cacheService.Gravar(CHAVE_CACHE_DADOS_PRODUTOS, produtosNaoCacheados, TimeSpan.FromHours(1));
                    return produtosCacheados.Concat(produtosNaoCacheados);
                }
            }

            return produtosCacheados;
        }



        private IEnumerable<Produto> ObterDadosDaBase(int[] idProdutos, IProgresso? progresso = null)
        {
            IEnumerable<DadosPrincipais> dadosPrincipais = _produtoDadosService.ObterDados(idProdutos);

            int[] idProdutosResgatados = dadosPrincipais.Select(_ => _.IdProduto).ToArray();

            if (progresso != null)
                progresso.NovaMensagem($"Obtendo caracteristicas de {idProdutosResgatados.Length} imóveis.");

            List<Caracteristica> caracteristicas = _produtoDadosService.ObterCaracteristicas(idProdutosResgatados).ToList();

            if (progresso != null)
                progresso.NovaMensagem($"Obtendo Tour virtual e Vídeos de {idProdutosResgatados.Length} imóveis.");

            IDictionary<int, string[]> urlTours = _produtoDadosService.ObterUrlTourVirtuais(idProdutosResgatados);
            IDictionary<int, string[]> urlVideos = _produtoDadosService.ObterUrlVideos(idProdutosResgatados);
            IEnumerable<Foto> fotos = _produtoDadosService.ObterFotos(idProdutosResgatados);

            if (progresso != null)
                progresso.NovaMensagem($"Preenchendo informações de {idProdutosResgatados.Length} imóveis.");

            List<Produto> produtos = new List<Produto>();
            foreach (DadosPrincipais dados in dadosPrincipais)
            {
                Produto imovel = new(dados);

                imovel.Caracteristicas = caracteristicas.Where(_ => _.IdProduto == dados.IdProduto);
                imovel.UrlTourVirtuais = urlTours.Where(_ => _.Key == dados.IdProduto).SelectMany(_ => _.Value);
                imovel.UrlVideos = urlVideos.Where(_ => _.Key == dados.IdProduto).SelectMany(_ => _.Value);
                imovel.Imagens = fotos.Where(_ => _.IdProduto == dados.IdProduto);

                yield return imovel;
            }
        }

        public int[] ObterFranquias(int idProduto)
        {
            throw new NotImplementedException();
        }
    }
}
