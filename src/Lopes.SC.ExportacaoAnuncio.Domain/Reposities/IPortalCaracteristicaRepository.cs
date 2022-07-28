using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IPortalCaracteristicaRepository
    {
        IEnumerable<PortalCaracteristica> Obter(Portal portal);
    }
}
