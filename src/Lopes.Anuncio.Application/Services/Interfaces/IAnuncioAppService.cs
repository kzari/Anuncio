using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.Services
{
    public interface IAnuncioAppService
    {
        IEnumerable<AnuncioCota> ObterAnunciosPorProdutos(int[] idProdutos, Portal? portal = null);
        IEnumerable<AnuncioCota> ObterAnunciosPorCotas(int[] idCotas);
        IEnumerable<AnuncioCota> ObterAnunciosPorPortais(Portal[] portais);
    }
}
