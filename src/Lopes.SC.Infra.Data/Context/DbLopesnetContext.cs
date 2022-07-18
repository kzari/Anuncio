using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lopes.SC.Infra.Data.Context
{
    public class DbLopesnetContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbLopesnetContext()
        {

        }
        public DbLopesnetContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<EmpresaApelidoPortal> EmpresasApelidoPortal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // TODO: connect to sql server with connection string from app settings
            if(Configuration != null)
                options.UseSqlServer(Configuration.GetConnectionString("DbLopesnet"));
            else
                options.UseSqlServer(@"Initial Catalog=dbLopesnet;Data Source=LPS-SI-DEV02\SQLCORP_HML;User id=usrapp;Password=Lopesnet2010;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new EmpresaApelidoPortalMap());
        }
    }
}
