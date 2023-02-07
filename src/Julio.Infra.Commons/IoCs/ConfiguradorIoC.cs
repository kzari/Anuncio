using Julio.Domain.Common.IoC;
using Julio.Domain.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Julio.Infra.Common
{
    /// <summary>
    /// Classe para configuração de injeção de dependências
    /// </summary>
    public class ConfiguradorIoC
    {
        private readonly IEnumerable<IConfiguracaoIoC> _configuracoes;

        public ConfiguradorIoC(IConfiguracaoIoC config) 
        {
            _configuracoes = new List<IConfiguracaoIoC>() { config };
        }
        public ConfiguradorIoC(params IConfiguracaoIoC[] configuracoes)
        {
            _configuracoes = configuracoes;
        }
        public ConfiguradorIoC(IEnumerable<IConfiguracaoIoC> configuracoes)
        {
            _configuracoes = configuracoes;
        }


        public IConfiguration Configuration { get; protected set; }
        public IServiceCollection ServiceCollection { get; protected set; }


        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider ??= ServiceCollection.BuildServiceProvider(validateScopes: true);


        public static ConfiguradorIoC ConfigurarServicos<TLogger>(IConfiguracaoIoC config,
                                                                  IConfiguration configuration = null,
                                                                  IServiceCollection services = null) where TLogger : class, ILogger
        {
            return ConfigurarServicos<TLogger>(new IConfiguracaoIoC[] { config }, configuration, services);
        }
        public static ConfiguradorIoC ConfigurarServicos<TLogger>(IConfiguracaoIoC[] config,
                                                                  IConfiguration configuration = null,
                                                                  IServiceCollection services = null) where TLogger : class, ILogger
        {
            return new ConfiguradorIoC(config).Configurar<TLogger>(configuration, services);
        }
        public ConfiguradorIoC Configurar<TLogger>(IConfiguration configuration = null, 
                                                   IServiceCollection services = null) where TLogger : class, ILogger
        {
            Configuration = configuration ?? BuildConfiguration();
            ServiceCollection = services ?? new ServiceCollection();

            RegistrarIConfiguration();
            RegistrarLog<TLogger>();

            foreach (IConfiguracaoIoC config in _configuracoes)
            {
                config.ConfigurarServicos(ServiceCollection, Configuration);
            }

            return this;
        }

        public IServiceScope CriarEscopo() => ServiceProvider.CreateScope();



        private void RegistrarIConfiguration()
        {
            ServiceCollection.AddSingleton(Configuration);
        }
        private void RegistrarLog<TLogger>() where TLogger : class, ILogger
        {
            ServiceCollection.AddSingleton<ILogger, TLogger>();
        }

        private static IConfigurationRoot BuildConfiguration(string caminhoArquivoJson = "appsettings.json")
        {
            IConfigurationBuilder configuration = new ConfigurationBuilder().AddJsonFile(caminhoArquivoJson);
            return configuration.Build();
        }
    }
}