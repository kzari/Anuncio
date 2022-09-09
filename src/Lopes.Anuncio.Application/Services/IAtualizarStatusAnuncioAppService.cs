using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;

namespace Lopes.Anuncio.Application.Services
{
    public interface IAtualizarStatusAnuncioAppService
    {
        Task<AtualizarStatusAnuncioResponse> Atualizar(AtualizarStatusAnuncioRequest request);
        IEnumerable<AtualizarStatusAnuncioResponse> Atualizar(IEnumerable<AtualizarStatusAnuncioRequest> atualizacoes);
    }
}