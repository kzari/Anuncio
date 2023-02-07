using Julio.Anuncio.Domain.Models;
using Julio.Anuncio.Domain.Models.DadosProduto;

namespace Julio.Anuncio.Application.Interfaces.DadosService
{
    public interface IProdutoDadosService
    {
        IEnumerable<Caracteristica> ObterCaracteristicas(int[] idProdutos);
        IEnumerable<DadosPrincipais> ObterDados(int[] idProdutos);
        IEnumerable<Foto> ObterFotos(int[] idProdutos);
        int[] ObterFranquias(int idProdutos);
        IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idProdutos);
        IDictionary<int, string[]> ObterUrlVideos(int[] idProdutos);
    }
}
