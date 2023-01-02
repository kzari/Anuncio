using Lopes.Botmaker.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Anuncio.Dados.Leitura.Context
{
    public class UsuarioLeituraContext : DbContext
    {
        public UsuarioLeituraContext(DbContextOptions<UsuarioLeituraContext> options) : base(options)
        {
        }

        public DbSet<UsuarioIntegracaoBotmakerDTO> UsuariosIntegracaoBotmaker { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UsuarioIntegracaoBotmakerDTO>().HasNoKey();
        }
    }
}