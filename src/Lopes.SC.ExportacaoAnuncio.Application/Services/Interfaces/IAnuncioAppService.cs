using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Application.Services
{
    public interface IAnuncioAppService
    {
        IEnumerable<Anuncio> ObterAnunciosPorImoveis(int[] idImoveis, Portal? portal = null);
        IEnumerable<Anuncio> ObterAnunciosPorCotas(int[] idCotas);
        IEnumerable<Anuncio> ObterAnunciosPorPortais(Portal[] portais);
    }
}
