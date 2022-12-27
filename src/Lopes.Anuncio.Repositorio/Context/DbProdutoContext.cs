using Lopes.Acesso.Data.Mappings;
using Lopes.Anuncio.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Anuncio.Repositorio.Context
{
    public class DbProdutoContext : DbContext
    {
        public DbProdutoContext(DbContextOptions<DbProdutoContext> options) : base(options)
        {
        }

        public DbSet<AnuncioAtualizacao> AnuncioAtualizacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AnuncioAtualizacaoMap());
        }
    }
}
