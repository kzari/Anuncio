using Lopes.Domain.Commons;
using Lopes.Anuncio.Domain.Commands.Requests;

namespace Lopes.Anuncio.Application.Interfaces
{
    public interface IAtualizacaoAppService
    {
        void Atualizar(AnuncioCotaRequest request, ILogger? logger);
    }
}