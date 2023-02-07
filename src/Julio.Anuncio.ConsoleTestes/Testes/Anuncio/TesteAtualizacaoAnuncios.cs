using Julio.Anuncio.Application.Interfaces;
using Julio.Anuncio.Domain.Commands.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Julio.ConsoleTestes.Testes.Anuncio
{
    public static class TesteAtualizacaoAnuncios
    {
        public static void Atualizar()
        {
            using (IServiceScope escopo = TesteBase.CriarEscopo())
            {
                IAtualizacaoAppService atualizarAnuncioService = escopo.ServiceProvider.GetService<IAtualizacaoAppService>();

                //atualizarAnuncioService.Atualizar(new AnuncioCotaRequest(idProdutos:new[] { 569521 }), null);
                atualizarAnuncioService.AtualizarAnuncios(new AnuncioCotaRequest(Julio.Anuncio.Domain.Enums.Portal.Zap), null);
            }
        }
    }
}
