using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.Commands.Responses;

namespace Julio.Anuncio.Application.Services
{
    public interface IRegistrarAtualizacaoAnunciosAppService
    {
        Task<AtualizarStatusAnuncioResponse> Registrar(RegistroAtualizacaoCommand request);
        IEnumerable<AtualizarStatusAnuncioResponse> Registrar(IEnumerable<RegistroAtualizacaoCommand> atualizacoes);
    }
}