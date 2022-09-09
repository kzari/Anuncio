using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;

namespace Lopes.Infra.Data.Repositories
{
    public class PortalCaracteristicaRepository : Repository<PortalCaracteristica>, IPortalCaracteristicaRepository
    {
        public PortalCaracteristicaRepository(DbProdutoContext context) : base(context)
        {
        }

        public IEnumerable<PortalCaracteristica> Obter(Portal portal)
        {
            return ObterTodos().Where(_ => _.Portal == portal).ToList();
        }
    }
}