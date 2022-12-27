using Lopes.Botmaker.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Botmaker.IoC
{
    public class BotmakerIoC
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public BotmakerIoC Build()
        {
            Configuration = BuildConfiguration();
            IServiceCollection services = ConfigurarServicos(Configuration);
            ServiceProvider = services.BuildServiceProvider(validateScopes: true);

            return this;
        }

        public IServiceScope CriarEscopo()
        {
            return ServiceProvider.CreateScope();
        }

        private IServiceCollection ConfigurarServicos(IConfiguration configuration, IServiceCollection services = null)
        {
            Configuration = configuration;
            services ??= new ServiceCollection();

            RegistrarIConfiguration(services, configuration);
            //RegistrarCache(services);
            //RegistrarLog<TLogger>(services);
            //RegistrarDadosServices(services);
            //RegistrarRepositorios(services);
            RegistrarAppServices(services);
            //RegistrarDbContexts(services, configuration);
            //RegistrarFabricas(services);
            //RegistrarDomainServices(services);
            //RegistrarHandlers(services);

            //services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

        private void RegistrarAppServices(IServiceCollection services)
        {
            string api = Configuration["Botmaker.Api"];
            string token = Configuration["Botmaker.Token"];
            services.AddSingleton<IBotmakerApiService>(new BotmakerApiService(token, api));

            services.AddScoped<IIntegracaoAppService, IntegracaoAppService>();
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            IConfigurationBuilder configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
            return configuration.Build();
        }
        private static void RegistrarIConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
        }
    }
}