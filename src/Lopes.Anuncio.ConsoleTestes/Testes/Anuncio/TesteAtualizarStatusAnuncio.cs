using Lopes.Anuncio.Application.Services;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Infra.ConsoleCommons.Log;
using Lopes.Infra.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes.Testes.Anuncio
{
    public static class TesteAtualizarStatusAnuncio
    {
        public static void AtualizarStatus()
        {
            ServiceCollection services = new();
            ServiceConfiguration.Configure<ConsoleLogger>(services);

            IServiceProvider provider = services.BuildServiceProvider(validateScopes: true);
            using (IServiceScope scope = provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IAtualizarStatusAnuncioAppService>();

                Task<AtualizarStatusAnuncioResponse> task = service.Atualizar(new AtualizarStatusAnuncioRequest(Portal.Zap, 569521, 1, AtualizacaoAcao.Atualizacao));

                AtualizarStatusAnuncioResponse response = task.Result;
            }
        }
    }
}
