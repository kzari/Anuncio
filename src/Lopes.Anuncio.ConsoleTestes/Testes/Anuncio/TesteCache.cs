using Lopes.Domain.Commons.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes.Testes.Anuncio
{
    public static class TesteCache
    {
        public static void CacheRedis()
        {
            using (IServiceScope scope = TesteBase.ObterServiceProvider().CreateScope())
            {
                var cacheService = scope.ServiceProvider.GetService<ICacheService>();

                cacheService.Gravar("AAA", "aaaAAAaaa", TimeSpan.FromSeconds(3600));

                string? dado = cacheService.Obter<string>("AAA");
            }
        }
    }
}
