using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Infra.Data.Context
{
    public class DbLopesnetContext : DbContext
    {
        public DbLopesnetContext(DbContextOptions<DbLopesnetContext> options) : base(options)
        {
        }

        public DbSet<EmpresaApelido> EmpresasApelidoPortal { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new EmpresaApelidoPortalMap());
        }
    }
}
