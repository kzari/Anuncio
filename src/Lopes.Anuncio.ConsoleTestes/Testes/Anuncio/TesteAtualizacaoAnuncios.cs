using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Commands.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.ConsoleTestes.Testes.Anuncio
{
    public static class TesteAtualizacaoAnuncios
    {
        public static void Atualizar()
        {
            using (IServiceScope escopo = TesteBase.CriarEscopo())
            {
                IAtualizacaoAppService atualizarAnuncioService = escopo.ServiceProvider.GetService<IAtualizacaoAppService>();

                //atualizarAnuncioService.Atualizar(new AnuncioCotaRequest(idImoveis:new[] { 569521 }), null);
                atualizarAnuncioService.Atualizar(new AnuncioCotaRequest(idCotas:new[] { 863 }), null);
            }
        }
    }
}
