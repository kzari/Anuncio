using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.ObjetosValor;

namespace Julio.Anuncio.Application.DadosService
{
    public interface IPortalCaracteristicaDadosService
    {
        IEnumerable<PortalCaracteristica> Obter(Portal portal);
    }
}
