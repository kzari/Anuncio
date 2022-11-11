using Lopes.Anuncio.Domain.Entidades;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Dados.Leitura.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Lopes.Anuncio.Dados.Leitura.Context
{
    public class DbProdutoLeituraContext : DbContext
    {
        public DbProdutoLeituraContext(DbContextOptions<DbProdutoLeituraContext> options) : base(options)
        {
        }


        public DbSet<AnuncioCota> Anuncios { get; set; }
        public DbSet<ProdutoFranquia> ProdutoFranquias { get; set; }
        public DbSet<DadosPrincipais> Produtos { get; set; }
        public DbSet<AnuncioAtualizacao> ProdutoAtualizacaoPortais { get; set; }
        public DbSet<Caracteristica> ProdutoCaracteristicas{ get; set; }
        public DbSet<TourVirtual> TourVirtuais { get; set; }
        public DbSet<Video> ProdutoVideos { get; set; }
        public DbSet<PortalCaracteristica> PortalCaracteristicas { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<CotaResumo> CotasResumo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CotaResumoMap());
            builder.ApplyConfiguration(new AnuncioMap());
            builder.ApplyConfiguration(new TourVirtualMap());
            builder.ApplyConfiguration(new VideoMap());
            builder.ApplyConfiguration(new PortalCaracteristicaMap());

            builder.Entity<ProdutoFranquia>().HasNoKey().ToView("VW_ProdutoFranquias");

            builder.Entity<DadosPrincipais>().HasKey(_ => _.IdProduto);
            builder.Entity<DadosPrincipais>().ToView("VW_AnuncioProdutoDados");

            builder.Entity<AnuncioAtualizacao>().ToTable("AnuncioAtualizacao").HasKey(_ => _.Id);

            builder.Entity<Caracteristica>().HasNoKey();

            builder.Entity<Foto>().ToView("VW_ProdutoImagensAnuncio");
            builder.Entity<Foto>().HasKey(_ => _.Id);

        }
    }
}
