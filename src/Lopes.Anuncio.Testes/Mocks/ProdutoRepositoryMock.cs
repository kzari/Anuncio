using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Lopes.Domain.Commons;

namespace Lopes.Anuncio.Domain.Testes.Mocks
{
    public class ProdutoRepositoryMock : IProdutoService
    {
        public IEnumerable<Produto> ObterDados(int[] idProdutos, IProgresso? progresso = null)
        {
            throw new NotImplementedException();
        }

        public int[] ObterFranquias(int idProduto)
        {
            return new[] { 1 };
        }
    }

    public class 
}