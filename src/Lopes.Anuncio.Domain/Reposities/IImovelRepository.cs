using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models;

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
