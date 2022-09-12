using Lopes.Anuncio.Application.Interfaces;
using Lopes.Infra.ConsoleCommons.Log;
using Lopes.Infra.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes.Testes.Anuncio
{
    public static class TesteAtualizacaoAnuncios
    {
        public static void Atualizar()
        {
            ServiceCollection services = new();
            ServiceConfiguration.Configure<ConsoleLogger>(services);

            IServiceProvider provider = services.BuildServiceProvider(validateScopes: true);
            using (IServiceScope scope = provider.CreateScope())
            {
                IAtualizarAnunciosAppService atualizarAnuncioService = scope.ServiceProvider.GetService<IAtualizarAnunciosAppService>();
                // atualizarAnuncioService.AtualizarPorPortais(new Portal[] { Portal.Zap }, null);

                //atualizarAnuncioService.AtualizarPorCotas(new [] { 48 }, null);

                atualizarAnuncioService.AtualizarPorImoveis(new[] { 569521 }, null, null);
            }
        }
    }
}
