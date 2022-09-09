using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IEmpresaApelidoPortalRepository
    {
        IEnumerable<EmpresaApelidoPortal> Obter();
    }
}
