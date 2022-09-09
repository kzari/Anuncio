using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Infra.Data.Repositories
{
    public class AnuncioRepository : Repository<AnuncioImovel>, IAnuncioRepository
    {
        public AnuncioRepository(DbProdutoContext context) : base(context)
        {
        }

        public IEnumerable<AnuncioImovel> ObterPorImoveis(int[] idImoveis, Portal? portal = null)
        {
            IQueryable<AnuncioImovel> query = base.ObterTodos().Where(_ => idImoveis.Contains(_.IdImovel) && (!portal.HasValue || _.Portal == portal.Value));
            return query.ToList();
        }

        public IEnumerable<AnuncioImovel> ObterPorCotas(int[] idCotas)
        {
            IQueryable<AnuncioImovel> query = base.ObterTodos().Where(_ => idCotas.Contains(_.IdCota));
            return query.ToList();
        }

        public IEnumerable<AnuncioImovel> ObterPorPortais(Portal[] idPortais)
        {
            IQueryable<AnuncioImovel> query = base.ObterTodos().Where(_ => idPortais.Contains(_.Portal));
            return query.ToList();
        }
    }
}
    