using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class AnuncioRepository : Repository<Anuncio>, IAnuncioRepository
    {
        public AnuncioRepository(DbProdutoContext context) : base(context)
        {
        }

        public IEnumerable<Anuncio> ObterPorImoveis(int[] idImoveis)
        {
            IQueryable<Anuncio> query = ObterTodos().Where(_ => idImoveis.Contains(_.IdImovel));
            return query.ToList();
        }

        public IEnumerable<Anuncio> ObterPorCotas(int[] idCotas)
        {
            IQueryable<Anuncio> query = ObterTodos().Where(_ => idCotas.Contains(_.IdCota));
            return query.ToList();
        }

        public IEnumerable<Anuncio> ObterPorPortais(Portal[] idPortais)
        {
            IQueryable<Anuncio> query = ObterTodos().Where(_ => idPortais.Contains(_.Portal));
            return query.ToList();
        }
    }
}
