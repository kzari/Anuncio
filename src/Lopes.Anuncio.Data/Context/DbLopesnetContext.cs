﻿using Lopes.Anuncio.Domain.Models;
using Lopes.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lopes.Infra.Data.Context
{
    public class DbLopesnetContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbLopesnetContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public DbSet<EmpresaApelidoPortal> EmpresasApelidoPortal { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DbLopesnet"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new EmpresaApelidoPortalMap());
        }
    }
}