using Microsoft.EntityFrameworkCore;

namespace Lopes.Anuncio.Dados.Leitura
{
    public class DadosServiceBase<TEntidade> where TEntidade : class
    {
        protected readonly DbContext Db;
        protected readonly DbSet<TEntidade> DbSet;

        public DadosServiceBase(DbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntidade>();
        }

        protected virtual IQueryable<TEntidade> ObterTodos()
        {
            return DbSet.AsNoTracking();
        }
    }
}