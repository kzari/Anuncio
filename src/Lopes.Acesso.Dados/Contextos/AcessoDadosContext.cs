using Lopes.Acesso.Dados.Mapeamentos;
using Lopes.Acesso.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Acesso.Dados
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