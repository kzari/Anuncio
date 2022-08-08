using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Domain.Reposities
{
    public interface IAnuncioRepository
    {
        IEnumerable<Models.Anuncio> ObterPorImoveis(int[] idImoveis, Portal? portal);
        IEnumerable<Models.Anuncio> ObterPorCotas(int[] idCotas);
        IEnumerable<Models.Anuncio> ObterPorPortais(Portal[] portais);
    }
}
