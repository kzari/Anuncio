using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.Imovel;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IImovelRepository
    {

        IEnumerable<DadosPrincipais> ObterDadosImoveis(int[] idImoveis);
        int[] ObterEmpresasImovel(int idImovel);
        IDictionary<int, string[]> ObterUrlTourVirtuais(int[] idImoveis);
        IDictionary<int, string[]> ObterUrlVideos(int[] idImoveis);
        IEnumerable<Caracteristica> ObterCaracteristicas(int[] idImoveis);
        IEnumerable<Fotos> ObterFotos(int[] idImoveis);
    }
}
