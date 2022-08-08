using Lopes.SC.Anuncio.Domain.Reposities;
using Microsoft.EntityFrameworkCore;

namespace Lopes.SC.Infra.Data.Repositories
{
    public class Repository<TEntidade> : IRepository<TEntidade> where TEntidade : class
    {
        protected readonly DbContext Db;
        protected readonly DbSet<TEntidade> DbSet;


        public Repository(DbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntidade>();
        }


        public virtual void Criar(TEntidade obj)
        {
            DbSet.Add(obj);
        }

        public virtual TEntidade ObterPorId(int id)
        {
            return DbSet.Find(id);
        }
        public virtual IQueryable<TEntidade> ObterTodos()
        {
            return DbSet;
        }

        public virtual void Alterar(TEntidade obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Excluir(int id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SalvarAlteracoes()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}