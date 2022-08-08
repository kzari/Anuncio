using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Domain.Reposities
{
    public interface IEmpresaApelidoPortalRepository
    {
        IEnumerable<EmpresaApelidoPortal> Obter();
    }
}
