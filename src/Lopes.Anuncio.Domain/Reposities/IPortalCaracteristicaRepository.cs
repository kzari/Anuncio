using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IPortalCaracteristicaRepository
    {
        IEnumerable<PortalCaracteristica> Obter(Portal portal);
    }
}
