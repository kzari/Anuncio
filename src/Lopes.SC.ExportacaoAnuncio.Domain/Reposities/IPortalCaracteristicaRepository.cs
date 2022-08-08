using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Domain.Reposities
{
    public interface IPortalCaracteristicaRepository
    {
        IEnumerable<PortalCaracteristica> Obter(Portal portal);
    }
}
