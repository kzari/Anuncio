using Julio.Anuncio.Domain.Models;
using Julio.Anuncio.Domain.Models.DadosProduto;
using Julio.Domain.Commons;

namespace Julio.Anuncio.Domain.Services
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
