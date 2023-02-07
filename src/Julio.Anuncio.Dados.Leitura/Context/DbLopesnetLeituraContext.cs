using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Anuncio.Dados.Leitura.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Julio.Anuncio.Dados.Leitura.Context
{
    public class DbLopesnetLeituraContext : DbContext
    {
        public DbLopesnetLeituraContext(DbContextOptions<DbLopesnetLeituraContext> options) : base(options)
        {
        }

        public DbSet<FranquiaApelido> EmpresasApelidoPortal { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new FranquiaApelidoPortalMap());
        }
    }
}
