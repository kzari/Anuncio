using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;

namespace Lopes.Infra.Data.Repositories
{
    public class EmpresaApelidoPortalRepository : Repository<EmpresaApelidoPortal>, IEmpresaApelidoPortalRepository
    {
        public EmpresaApelidoPortalRepository(DbLopesnetContext context) : base(context)
        {
        }

        public IEnumerable<EmpresaApelidoPortal> Obter() => ObterTodos().ToList();
    }
}
