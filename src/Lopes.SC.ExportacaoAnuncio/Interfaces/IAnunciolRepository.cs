using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Interfaces
{
    public interface IAnunciolRepository
    {
        IEnumerable<Anuncio> ObterPorCotas(int[] idCotas);
        IEnumerable<Anuncio> ObterPorImoveis(int[] idImoveis);
        IEnumerable<Anuncio> ObterPorPortais(Portal[] idPortais);
    }
}
