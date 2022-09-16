using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.DadosService
{
    public interface IPortalCaracteristicaDadosService
    {
        IEnumerable<PortalCaracteristica> Obter(Portal portal);
    }
}
