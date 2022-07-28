using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.Infra.Data.Context;

namespace Lopes.SC.Infra.Data.Repositories
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