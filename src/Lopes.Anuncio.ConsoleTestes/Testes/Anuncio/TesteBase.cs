using Lopes.Infra.ConsoleCommons.Log;
using Lopes.Infra.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes.Testes.Anuncio
{
    public static class TesteBase
    {
        public static IConfigurationRoot ObterConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .Build();
        }
        public static IServiceProvider ObterServiceProvider() => ObterServiceProvider(ObterConfiguration());
        public static IServiceProvider ObterServiceProvider(IConfiguration configuration)
        {
            IServiceCollection services = ConfiguracaoServicos.ConfigurarServicos<ConsoleLogger>(configuration);
            return services.BuildServiceProvider(validateScopes: true);
        }
        public static IServiceScope CriarEscopo()
        {
            return ObterServiceProvider().CreateScope();
        }
    }
}
