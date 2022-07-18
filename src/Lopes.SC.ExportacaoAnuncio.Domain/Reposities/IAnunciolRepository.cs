using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IAnuncioRepository
    {
        IEnumerable<Anuncio> ObterPorImoveis(int[] idImoveis);
        IEnumerable<Anuncio> ObterPorCotas(int[] idCotas);
        IEnumerable<Anuncio> ObterPorPortais(Portal[] portais);
    }
}
