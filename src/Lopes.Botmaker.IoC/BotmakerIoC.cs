using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Dados.Leitura.DadosService;
using Lopes.Botmaker.Application.DadosServices;
using Lopes.Botmaker.Application.Services;
using Lopes.Domain.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Botmaker.IoC
{
    public class BotmakerIoC
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public BotmakerIoC Build<TLogger>() where TLogger : class, ILogger
        {
            Configuration = BuildConfiguration();
            IServiceCollection services = ConfigurarServicos<TLogger>(Configuration);
            ServiceProvider = services.BuildServiceProvider(validateScopes: true);

            return this;
        }

        public IServiceScope CriarEscopo() => ServiceProvider.CreateScope();

        private IServiceCollection ConfigurarServicos<TLogger>(IConfiguration configuration, IServiceCollection services = null) where TLogger : class, ILogger
        {
            Configuration = configuration;
            services ??= new ServiceCollection();

            RegistrarIConfiguration(services, configuration);
            //RegistrarCache(services);
            RegistrarLog<TLogger>(services);
            RegistrarDadosServices(services, configuration);
            //RegistrarRepositorios(services);
            RegistrarAppServices(services);
            //RegistrarDbContexts(services, configuration);
            //RegistrarFabricas(services);
            //RegistrarDomainServices(services);
            //RegistrarHandlers(services);

            //services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

        private void RegistrarDadosServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsuarioLeituraContext>(_ => _.UseSqlServer(configuration.GetConnectionString("DbLopesnet")), ServiceLifetime.Transient);
        }

        private void RegistrarAppServices(IServiceCollection services)
        {
            string api = Configuration["Botmaker.Api"];
            string token = Configuration["Botmaker.Token"];
            services.AddSingleton<IBotmakerApiService>(new BotmakerApiService(token, api));

            services.AddScoped<IIntegracaoAppService, IntegracaoAppService>();
            services.AddScoped<IIntegracaoBotmakerDadosService, IntegracaoBotmakerDadosService>();
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
        protected virtual void RegistrarLog<TLogger>(IServiceCollection services) where TLogger : class, ILogger
        {
            services.AddSingleton<ILogger, TLogger>();
        }
    }
}