using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lopes.SC.Infra.Data.Context
{
    public class DbProdutoContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbProdutoContext()
        {
        }

        public DbSet<Anuncio> Anuncios { get; set; }
        public DbSet<ImovelEmpresa> ImovelEmpresas { get; set; }
        public DbSet<DadosPrincipais> Imoveis { get; set; }
        public DbSet<AnuncioAtualizacao> ImovelAtualizacaoPortais { get; set; }
        public DbSet<Caracteristica> ImovelCaracteristicas{ get; set; }
        public DbSet<TourVirtual> TourVirtuais { get; set; }
        public DbSet<Video> ImovelVideos { get; set; }
        public DbSet<ExportacaoAnuncio.Domain.Models.PortalCaracteristica> PortalCaracteristicas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // TODO: connect to sql server with connection string from app settings
            options.UseSqlServer(@"Initial Catalog=dbproduto;Data Source=LPS-SI-DEV02\SQLCORP_HML;User id=usrapp;Password=Lopesnet2010;");
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
        }
    }
}
