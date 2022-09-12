using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Application.Services;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Lopes.Infra.Data.Context;
using Lopes.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Lopes.Domain.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using Lopes.Infra.Data.RepositoriosGravacao;

namespace Lopes.Infra.IoC
{
    public abstract class ServiceConfiguration
    {
        public static void Configure<TLogger>(IServiceCollection services) where TLogger : class, ILogger
        {
            services.AddMemoryCache();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddMemoryCache();

            ConfigurarLog<TLogger>(services);

            services.AddTransient<IPortalAtualizadorFactory, PortalAtualizadorFactory>();

            ConfigurarAppServices(services);
            ConfigurarRepositorios(services);
            ConfigurarDomainServices(services);
            ConfigurarDbContexts(services);
        }


        private static void ConfigurarOutrosServicos(IServiceCollection services)
        {
            services.AddTransient<IPortalAtualizadorFactory, PortalAtualizadorFactory>();

            // Build configuration
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);
        }

        private static void ConfigurarDbContexts(IServiceCollection services)
        {
            services.AddDbContext<DbProdutoContext>(options =>  options.UseSqlServer(_ => _.CommandTimeout(3600)), ServiceLifetime.Transient);
            services.AddDbContext<DbLopesnetContext>(options => options.UseSqlServer(_ => _.CommandTimeout(3600)), ServiceLifetime.Transient);
        }

        private static void ConfigurarLog<TLogger>(IServiceCollection services)  where TLogger : class, ILogger
        {
            services.AddSingleton<ILogger, TLogger>();
        }

        private static void ConfigurarDomainServices(IServiceCollection services)
        {
            services.AddTransient<IStatusAnuncioService, StatusAnuncioService>();
        }

        private static void ConfigurarAppServices(IServiceCollection services)
        {
            services.AddTransient<IAnuncioAppService, AnuncioAppService>();
            services.AddTransient<IAtualizarAnunciosAppService, AtualizarAnunciosAppService>();
            services.AddTransient<IDadosImovelAppService, DadosImovelAppService>();
            services.AddTransient<IAtualizarStatusAnuncioAppService, AtualizarStatusAnuncioAppService>();
            
        }
        private static void ConfigurarRepositorios(IServiceCollection services)
        {
            services.AddTransient<IEmpresaApelidoPortalRepository, EmpresaApelidoPortalRepository>();
            services.AddTransient<IImovelRepository, ImovelRepository>();
            services.AddTransient<IAnuncioRepository, AnuncioRepository>();
            services.AddTransient<IAnuncioStatusRepositorioGravacao, AnuncioStatusRepositorioGravacao>();
            services.AddTransient<IPortalCaracteristicaRepository, PortalCaracteristicaRepository>();
        }
    }
}