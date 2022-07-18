using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Application.Services;
using Lopes.SC.ExportacaoAnuncio.Application.Services.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.Infra.Data.Context;
using Lopes.SC.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.SC.Infra.IoC
{
    public abstract class ServiceConfiguration
    {
        private const string CAMINHO_PASTA_XMLs = "C:/temp/portais"; //TODO: colocar no arquivo de configuração 


        public static IServiceProvider ConfigureServices<TLogger>(IServiceCollection services) where TLogger : class, ILogger
        {
            services.AddMemoryCache();

            ConfigurarLog<TLogger>(services);

            ConfigurarAppServices(services);
            ConfigurarRepositorios(services);
            ConfigurarDomainServices(services);

            services.AddDbContext<DbProdutoContext>();
            services.AddDbContext<DbLopesnetContext>();

            return services.BuildServiceProvider(validateScopes: true);
        }



        private static void ConfigurarLog<TLogger>(IServiceCollection services)  where TLogger : class, ILogger
        {
            services.AddSingleton<ILogger, TLogger>();
        }

        private static void ConfigurarDomainServices(IServiceCollection services)
        {
            services.AddScoped<IStatusAnuncioService, StatusAnuncioService>();
        }

        private static void ConfigurarAppServices(IServiceCollection services)
        {
            services.AddScoped<IAnuncioAppService, AnuncioAppService>();
            services.AddScoped<IAtualizarAnunciosAppService, AtualizarAnunciosAppService>();
            services.AddScoped<IDadosImovelAppService, DadosImovelAppService>();
            services.AddScoped<IImovelXMLAppService, ImovelXMLAppService>();

            services.AddSingleton<IImovelXMLAppService>(_ =>
                new ImovelXMLAppService(CAMINHO_PASTA_XMLs,
                                        _.GetService<IEmpresaApelidoPortalRepository>(),
                                        _.GetService<ILogger>()));
        }

        private static void ConfigurarRepositorios(IServiceCollection services)
        {
            services.AddScoped<IEmpresaApelidoPortalRepository, EmpresaApelidoPortalRepository>();
            services.AddScoped<IImovelRepository, ImovelRepository>();
            services.AddScoped<IAnuncioRepository, AnuncioRepository>();
            services.AddScoped<IImovelAtualizacaoPortaisRepository, ImovelAtualizacaoPortaisRepository>();
            //services.AddScoped<IRepository<>, Repository<TEntity>>();
        }
    }
}