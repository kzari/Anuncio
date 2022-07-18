using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class AnuncioRepository : Repository<Anuncio>, IAnunciolRepository
    {
        public AnuncioRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Anuncio> ObterPorImoveis(int[] idImoveis)
        {
            IQueryable<Anuncio> query = GetAll().Where(_ => idImoveis.Contains(_.IdImovel));
            return query.ToList();
        }

        public IEnumerable<Anuncio> ObterPorCotas(int[] idCotas)
        {
            IQueryable<Anuncio> query = GetAll().Where(_ => idCotas.Contains(_.IdCota));
            return query.ToList();
        }

        public IEnumerable<Anuncio> ObterPorPortais(Portal[] idPortais)
        {
            IQueryable<Anuncio> query = GetAll().Where(_ => idPortais.Contains(_.Portal));
            return query.ToList();
        }
    }
}
