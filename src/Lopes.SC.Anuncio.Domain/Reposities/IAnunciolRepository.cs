using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Domain.Reposities
{
    public interface IAnuncioRepository
    {
        IEnumerable<AnuncioImovel> ObterPorImoveis(int[] idImoveis, Portal? portal);
        IEnumerable<AnuncioImovel> ObterPorCotas(int[] idCotas);
        IEnumerable<AnuncioImovel> ObterPorPortais(Portal[] portais);
    }
}
