using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Application.Services;
using Lopes.SC.ExportacaoAnuncio.Application.Services.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.Infra.Data.Context;
using Lopes.SC.Infra.Data.Repositories;
using Lopes.SC.Infra.XML;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.SC.Infra.IoC
{
    public abstract class ServiceConfiguration
    {
        private const string CAMINHO_PASTA_XMLs = "C:/temp/portais_novo"; //TODO: colocar no arquivo de configuração 


        public static IServiceProvider ConfigureServices<TLogger>(IServiceCollection services) where TLogger : class, ILogger
        {
            services.AddMemoryCache();

            ConfigurarLog<TLogger>(services);

            ConfigurarAppServices(services);
            ConfigurarRepositorios(services);
            ConfigurarDomainServices(services);
            ConfigureDbContexts(services);

            return services.BuildServiceProvider(validateScopes: true);
        }

        private static void ConfigureDbContexts(IServiceCollection services)
        {
            services.AddDbContext<DbProdutoContext>(ServiceLifetime.Transient);
            services.AddDbContext<DbLopesnetContext>(ServiceLifetime.Transient);
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
            services.AddTransient<IAtualizarAnunciosAppService, AtualizarAnunciosXMLAppService>();
            services.AddTransient<IDadosImovelAppService, DadosImovelAppService>();
            services.AddTransient<IImovelXMLAppService, ImovelXMLAppService>();
            services.AddTransient<IPortalXMLBuilder, PortalXMLBuilder>();

            services.AddTransient<IImovelXMLAppService>(_ =>
                new ImovelXMLAppService(CAMINHO_PASTA_XMLs,
                                        _.GetService<IEmpresaApelidoPortalRepository>(),
                                        _.GetService<ILogger>()));
        }

        private static void ConfigurarRepositorios(IServiceCollection services)
        {
            services.AddTransient<IEmpresaApelidoPortalRepository, EmpresaApelidoPortalRepository>();
            services.AddTransient<IImovelRepository, ImovelRepository>();
            services.AddTransient<IAnuncioRepository, AnuncioRepository>();
            services.AddTransient<IImovelAtualizacaoPortaisRepository, ImovelAtualizacaoPortaisRepository>();
            //services.AddScoped<IRepository<>, Repository<TEntity>>();
        }
    }
}