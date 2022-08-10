using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;
using Lopes.SC.Anuncio.Domain.Reposities;
using Lopes.SC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class AnuncioRepository : Repository<Anuncio.Domain.Models.AnuncioImovel>, IAnuncioRepository
    {
        public AnuncioRepository(DbProdutoContext context) : base(context)
        {
        }

        public IEnumerable<Anuncio.Domain.Models.AnuncioImovel> ObterPorImoveis(int[] idImoveis, Portal? portal = null)
        {
            IQueryable<Anuncio.Domain.Models.AnuncioImovel> query = base.ObterTodos().Where(_ => idImoveis.Contains(_.IdImovel) && (!portal.HasValue || _.Portal == portal.Value));
            return query.ToList();
        }

        public IEnumerable<Anuncio.Domain.Models.AnuncioImovel> ObterPorCotas(int[] idCotas)
        {
            IQueryable<Anuncio.Domain.Models.AnuncioImovel> query = base.ObterTodos().Where(_ => idCotas.Contains(_.IdCota));
            return query.ToList();
        }

        public IEnumerable<Anuncio.Domain.Models.AnuncioImovel> ObterPorPortais(Portal[] idPortais)
        {
            IQueryable<Anuncio.Domain.Models.AnuncioImovel> query = base.ObterTodos().Where(_ => idPortais.Contains(_.Portal));
            return query.ToList();
        }
    }
}
    