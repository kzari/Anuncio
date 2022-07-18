using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Reposities
{
    public interface IEmpresaApelidoPortalRepository
    {
        IEnumerable<EmpresaApelidoPortal> Obter();
    }
}
