using Lopes.Domain.Common.IoC;
using Lopes.Domain.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Infra.Common
{
    public class IoC
    {
        private readonly IEnumerable<IBaseIoC> _configuracoes;

        public IoC(IBaseIoC config) 
        {
            _configuracoes = new List<IBaseIoC>() { config };
        }
        public IoC(params IBaseIoC[] configuracoes)
        {
            _configuracoes = configuracoes;
        }
        public IoC(IEnumerable<IBaseIoC> configuracoes)
        {
            _configuracoes = configuracoes;
        }


        public IConfiguration Configuration { get; protected set; }
        public IServiceCollection ServiceCollection { get; protected set; }


        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider ??= ServiceCollection.BuildServiceProvider(validateScopes: true);

        public IServiceScope CriarEscopo() => ServiceProvider.CreateScope();


        public static IoC ConfigurarServicos<TLogger>(IBaseIoC config, 
                                                      IConfiguration configuration = null, 
                                                      IServiceCollection services = null) where TLogger : class, ILogger
        {
            return new IoC(config).Configurar<TLogger>(configuration, services);
        }
        public IoC Configurar<TLogger>(IConfiguration configuration = null, IServiceCollection services = null) where TLogger : class, ILogger
        {
            Configuration = configuration ?? BuildConfiguration();
            ServiceCollection = services ?? new ServiceCollection();

            RegistrarIConfiguration();
            RegistrarLog<TLogger>();

            foreach (IBaseIoC config in _configuracoes)
            {
                config.ConfigurarServicos(ServiceCollection, Configuration);
            }

            return this;
        }



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