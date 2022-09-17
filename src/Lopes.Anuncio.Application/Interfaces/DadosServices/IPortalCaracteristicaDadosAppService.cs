using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.DadosService
{
    public interface IPortalCaracteristicaDadosAppService
    {
        IEnumerable<PortalCaracteristica> Obter(Portal portal);
    }
}
