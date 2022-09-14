using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Infra.Data.Repositories
{
    public class AnuncioRepository : Repository<AnuncioCota>, IAnuncioRepository
    {
        public AnuncioRepository(DbProdutoContext context) : base(context)
        {
        }

        public IEnumerable<AnuncioCota> Obter(AnuncioCotaRequest request)
        {
            IQueryable<AnuncioCota> query = base.ObterTodos();

            if (request.IdImoveis != null && request.IdImoveis.Any())
                query = query.Where(_ => request.IdImoveis.Contains(_.IdImovel));

            if (request.IdCotas != null && request.IdCotas.Any())
                query = query.Where(_ => request.IdCotas.Contains(_.IdCota));

            if (request.Portais != null && request.Portais.Any())
                query = query.Where(_ => request.Portais.Contains(_.Portal));

            return query.ToList();
        }
    }
}
    