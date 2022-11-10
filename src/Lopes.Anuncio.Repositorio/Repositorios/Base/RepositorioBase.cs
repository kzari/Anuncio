using Microsoft.EntityFrameworkCore;

namespace Lopes.Acesso.Data.Repositories
{
    public class RepositorioBase<TEntidade> where TEntidade : class
    {
        protected readonly DbContext Db;
        protected readonly DbSet<TEntidade> DbSet;

        public RepositorioBase(DbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntidade>();
        }


        public virtual void Criar(TEntidade obj)
        {
            DbSet.Add(obj);
        }

        public virtual void Alterar(TEntidade obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Excluir(int id)
        {
            TEntidade entidade = DbSet.Find(id);
            if(entidade != null)
                DbSet.Remove(entidade);
        }

        public int SalvarAlteracoes()
        {
            return Db.SaveChanges();
        }

        protected virtual TEntidade? ObterPorId(int id)
        {
            return DbSet.Find(id);
        }
        protected virtual IQueryable<TEntidade> ObterTodos(bool noTracking = true)
        {
            return noTracking
                ? DbSet.AsNoTracking()
                : DbSet;
        }
    }
}