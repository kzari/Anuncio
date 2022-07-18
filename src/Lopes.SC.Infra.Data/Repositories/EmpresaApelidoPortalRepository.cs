using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.Infra.Data.Context;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class EmpresaApelidoPortalRepository : Repository<EmpresaApelidoPortal>, IEmpresaApelidoPortalRepository
    {
        public EmpresaApelidoPortalRepository(DbLopesnetContext context) : base(context)
        {
        }

        public IEnumerable<EmpresaApelidoPortal> Obter() => ObterTodos().ToList();
    }
}
