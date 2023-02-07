using Julio.Acesso.Dados.Mapeamentos;
using Julio.Acesso.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Julio.Acesso.Dados
{
    public class AcessoDadosContext : DbContext
    {
        public AcessoDadosContext(DbContextOptions<AcessoDadosContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UsuarioMap());
        }
    }
}