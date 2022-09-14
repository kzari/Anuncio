using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;

namespace Lopes.Anuncio.Application.Services
{
    public interface IRegistrarAtualizacaoAnunciosAppService
    {
        Task<AtualizarStatusAnuncioResponse> Registrar(RegistroAtualizacaoCommand request);
        IEnumerable<AtualizarStatusAnuncioResponse> Registrar(IEnumerable<RegistroAtualizacaoCommand> atualizacoes);
    }
}