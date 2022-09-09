using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Application.Services
{
    public interface IAnuncioAppService
    {
        IEnumerable<AnuncioImovel> ObterAnunciosPorImoveis(int[] idImoveis, Portal? portal = null);
        IEnumerable<AnuncioImovel> ObterAnunciosPorCotas(int[] idCotas);
        IEnumerable<AnuncioImovel> ObterAnunciosPorPortais(Portal[] portais);
    }
}
