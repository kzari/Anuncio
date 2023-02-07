using Microsoft.EntityFrameworkCore;

namespace Julio.Anuncio.Repositorio.Context
{
    public class DbLopesnetContext : DbContext
    {
        public DbLopesnetContext(DbContextOptions<DbLopesnetContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
