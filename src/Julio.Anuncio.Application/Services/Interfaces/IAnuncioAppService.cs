using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.ObjetosValor;

namespace Julio.Anuncio.Application.Services
{
    public interface IAnuncioAppService
    {
        IEnumerable<AnuncioCota> ObterAnunciosPorProdutos(int[] idProdutos, Portal? portal = null);
        IEnumerable<AnuncioCota> ObterAnunciosPorCotas(int[] idCotas);
        IEnumerable<AnuncioCota> ObterAnunciosPorPortais(Portal[] portais);
    }
}
