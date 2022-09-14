using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;

namespace Lopes.Infra.Data.Repositories
{
    public class EmpresaApelidoPortalRepository : Repository<EmpresaApelido>, IEmpresaApelidoPortalRepository
    {
        public EmpresaApelidoPortalRepository(DbLopesnetContext context) : base(context)
        {
        }

        public IEnumerable<EmpresaApelido> Obter() => ObterTodos().ToList();
    }
}
