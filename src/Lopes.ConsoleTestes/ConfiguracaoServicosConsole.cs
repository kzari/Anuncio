using Lopes.Domain.Commons.Cache;
using Lopes.Infra.Cache;
using Lopes.Anuncio.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes
{
    internal class ConfiguracaoServicosConsole : ConfiguracaoServicos
    {
        protected override void RegistrarCache(IServiceCollection services, IConfiguration configuration)
        {
            string configuracaoRedis = configuration["Redis.Conexao"];
            if (!string.IsNullOrEmpty(configuracaoRedis))
            {
                services.AddStackExchangeRedisCache(options => options.Configuration = configuracaoRedis);
                services.AddSingleton<ICacheService, CacheDistribuidoService>();
            }
            else
            {
                Console.WriteLine("A configuração do servidor do Redis está vazia, usando cache em memória.");
                base.RegistrarCache(services, configuration);
            }
        }
    }
}
