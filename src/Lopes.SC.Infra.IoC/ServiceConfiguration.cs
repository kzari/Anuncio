using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Application.Services;
using Lopes.SC.ExportacaoAnuncio.Domain.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.Infra.Data.Repositories;

namespace Lopes.SC.Infra.IoC
{
    public abstract class ServiceConfiguration
    {
        private const string CAMINHO_PASTA_XMLs = "C:/temp/portais"; //TODO: colocar no arquivo de configuração 


        public static IServiceCollection ConfigureServices<TLogger>(IServiceCollection services) where TLogger : ILogger
        {
            services.AddMemoryCache();

            ConfigurarLog<TLogger>(services);

            ConfigurarAppServices(services);
            ConfigurarRepositorios(services);
            ConfigurarDomainServices(services);

            

            return services;
        }



        private static void ConfigurarLog<TLogger>(IServiceCollection services)
        {
            services.AddSingleton<ILogger, TLogger>();
        }

        private static void ConfigurarDomainServices(IServiceCollection services)
        {
            services.AddScoped<IStatusAnuncioService, StatusAnuncioService>();
        }

        private static void ConfigurarAppServices(IServiceCollection services)
        {
            services.AddScoped<IAtualizarAnunciosAppService, AtualizarAnunciosAppService>();
            services.AddScoped<IDadosImovelAppService, DadosImovelAppService>();

            services.AddSingleton<IImovelXMLAppService>(_ =>
                new ImovelXMLAppService(CAMINHO_PASTA_XMLs,
                                        _.GetService<IEmpresaApelidoPortalRepository>(),
                                        _.GetService<ILogger>()));
        }

        private static void ConfigurarRepositorios(IServiceCollection services)
        {
            services.AddScoped<IEmpresaApelidoPortalRepository, EmpresaApelidoPortalRepository>();
            services.AddScoped<IImovelRepository, ImovelRepository>();
            services.AddScoped<IAnunciolRepository, AnuncioRepository>();
            services.AddScoped<IImovelAtualizacaoPortaisRepository, ImovelAtualizacaoPortaisRepository>();
            //services.AddScoped<IRepository<>, Repository<TEntity>>();
        }
    }
}