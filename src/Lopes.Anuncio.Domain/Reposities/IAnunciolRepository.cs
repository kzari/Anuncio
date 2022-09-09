using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IAnuncioRepository
    {
        IEnumerable<AnuncioImovel> ObterPorImoveis(int[] idImoveis, Portal? portal);
        IEnumerable<AnuncioImovel> ObterPorCotas(int[] idCotas);
        IEnumerable<AnuncioImovel> ObterPorPortais(Portal[] portais);
    }
}
