using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models;
using Lopes.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lopes.Infra.Data.Context
{
    public class DbProdutoContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbProdutoContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public DbSet<Anuncio.Domain.Models.AnuncioImovel> Anuncios { get; set; }
        public DbSet<ImovelEmpresa> ImovelEmpresas { get; set; }
        public DbSet<DadosPrincipais> Imoveis { get; set; }
        public DbSet<AnuncioAtualizacao> ImovelAtualizacaoPortais { get; set; }
        public DbSet<Caracteristica> ImovelCaracteristicas{ get; set; }
        public DbSet<TourVirtual> TourVirtuais { get; set; }
        public DbSet<Video> ImovelVideos { get; set; }
        public DbSet<PortalCaracteristica> PortalCaracteristicas { get; set; }
        public DbSet<Fotos> ImovelImagens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DbProduto"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AnuncioMap());
            builder.ApplyConfiguration(new TourVirtualMap());
            builder.ApplyConfiguration(new VideoMap());
            builder.ApplyConfiguration(new PortalCaracteristicaMap());

            builder.Entity<ImovelEmpresa>().HasNoKey().ToView("VW_ImovelEmpresas");

            builder.Entity<DadosPrincipais>().HasKey(_ => _.IdImovel);
            builder.Entity<DadosPrincipais>().ToView("VW_AnuncioImovelDados");

            builder.Entity<AnuncioAtualizacao>().ToTable("AnuncioAtualizacao").HasKey(_ => _.Id);

            builder.Entity<Caracteristica>().HasNoKey();

            builder.Entity<Fotos>().ToView("VW_ImovelImagensAnuncio");
            builder.Entity<Fotos>().HasKey(_ => _.Id);

        }
    }
}
