using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{
    public interface IAnuncioAppService
    {
        IEnumerable<Anuncio> ObterAnunciosPorImoveis(int[] idImoveis, Portal? portal = null);
        IEnumerable<Anuncio> ObterAnunciosPorCotas(int[] idCotas);
        IEnumerable<Anuncio> ObterAnunciosPorPortais(Portal[] portais);
    }
}
