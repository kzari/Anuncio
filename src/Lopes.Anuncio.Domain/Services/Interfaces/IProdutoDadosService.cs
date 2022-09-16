using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;

namespace Lopes.Anuncio.Domain.Services
{
    public interface IProdutoDadosService
    {
        IEnumerable<DadosPrincipais> ObterDados(int[] idProdutos);
        /// <summary>
        /// Retorna o id das filiais que o produto pertence
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        int[] ObterFranquias(int idProduto);
        IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idProdutos);
        IDictionary<int, string[]> ObterUrlVideos(int[] idProdutos);
        IEnumerable<Caracteristica> ObterCaracteristicas(int[] idProdutos);
        IEnumerable<Foto> ObterFotos(int[] idProdutos);
    }
}
