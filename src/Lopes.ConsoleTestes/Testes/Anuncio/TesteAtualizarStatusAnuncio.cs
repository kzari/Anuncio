using Lopes.Anuncio.Application.Services;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes.Testes.Anuncio
{
    public static class TesteAtualizarStatusAnuncio
    {
        public static void AtualizarStatus()
        {
            using (IServiceScope scope = TesteBase.ObterServiceProvider().CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IRegistrarAtualizacaoAnunciosAppService>();

                Task<AtualizarStatusAnuncioResponse> task = service.Registrar(new RegistroAtualizacaoCommand(Portal.Zap, 569521, 1, AtualizacaoAcao.Atualizacao));

                AtualizarStatusAnuncioResponse response = task.Result;
            }
        }
    }
}
