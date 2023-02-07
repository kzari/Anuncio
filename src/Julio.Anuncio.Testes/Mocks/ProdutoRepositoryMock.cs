using Julio.Anuncio.Domain.Models;
using Julio.Anuncio.Domain.Models.DadosProduto;
using Julio.Anuncio.Domain.Reposities;
using Julio.Anuncio.Domain.Services;
using Julio.Domain.Commons;

namespace Julio.Anuncio.Domain.Testes.Mocks
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
}