using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Application.Services
{
    public interface IAnuncioAppService
    {
        IEnumerable<AnuncioImovel> ObterAnunciosPorImoveis(int[] idImoveis, Portal? portal = null);
        IEnumerable<AnuncioImovel> ObterAnunciosPorCotas(int[] idCotas);
        IEnumerable<AnuncioImovel> ObterAnunciosPorPortais(Portal[] portais);
    }
}
