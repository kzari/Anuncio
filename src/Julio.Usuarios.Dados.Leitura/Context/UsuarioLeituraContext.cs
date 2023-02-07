using Julio.Botmaker.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Julio.Anuncio.Dados.Leitura.Context
{
    public class UsuarioLeituraContext : DbContext
    {
        public UsuarioLeituraContext(DbContextOptions<UsuarioLeituraContext> options) : base(options)
        {
        }

        public DbSet<DadosUsuarioDTO> UsuariosIntegracaoBotmaker { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DadosUsuarioDTO>().HasNoKey();
        }
    }
}