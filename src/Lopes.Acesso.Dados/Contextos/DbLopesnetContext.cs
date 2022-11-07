using Lopes.Acesso.Dados.Mapeamentos;
using Lopes.Acesso.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Acesso.Dados
{
    public class DbLopesnetContext : DbContext
    {
        public DbLopesnetContext(DbContextOptions<DbLopesnetContext> options) : base(options)
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