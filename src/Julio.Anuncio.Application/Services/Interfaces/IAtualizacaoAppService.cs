using Julio.Domain.Commons;
using Julio.Anuncio.Domain.Commands.Requests;

namespace Julio.Anuncio.Application.Interfaces
{
    public interface IAtualizacaoAppService
    {
        void AtualizarAnuncios(AnuncioCotaRequest request, ILogger? logger);
    }
}