using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Domain.Commons;

namespace Lopes.Anuncio.Domain.Services
{
    public interface IProdutoService
    {
        IEnumerable<Produto> ObterDados(int[] idProdutos, IProgresso? progresso = null);
        /// <summary>
        /// Retorna o id das filiais que o produto pertence
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        int[] ObterFranquias(int idProduto);
    }
}
