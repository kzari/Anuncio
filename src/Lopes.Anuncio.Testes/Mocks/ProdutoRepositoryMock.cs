using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;

namespace Lopes.Anuncio.Domain.Testes.Mocks
{
    public class ProdutoRepositoryMock : IProdutoDadosService
    {
        public IEnumerable<Caracteristica> ObterCaracteristicas(int[] idProdutos)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DadosPrincipais> ObterDadosProdutos(int[] idProdutos)
        {
            throw new NotImplementedException();
        }

        public DadosPrincipais ObterDadosImovel(int idImovel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Foto> ObterFotos(int[] idProdutos)
        {
            throw new NotImplementedException();
        }

        public IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idProdutos)
        {
            throw new NotImplementedException();
        }

        public IDictionary<int, string[]> ObterUrlVideos(int[] idProdutos)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DadosPrincipais> ObterDados(int[] idProdutos)
        {
            throw new NotImplementedException();
        }

        public int[] ObterFranquias(int idProduto)
        {
            throw new NotImplementedException();
        }
    }
}