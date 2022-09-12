using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;
using MediatR;

namespace Lopes.Anuncio.Application.Services
{
    public class AtualizarStatusAnuncioAppService : IAtualizarStatusAnuncioAppService
    {
        private readonly IMediator _mediator;

        public AtualizarStatusAnuncioAppService(IMediator handler)
        {
            _mediator = handler;
        }


        public Task<AtualizarStatusAnuncioResponse> Atualizar(AtualizarStatusAnuncioRequest request)
        {
            return _mediator.Send(request);
        }


        public IEnumerable<AtualizarStatusAnuncioResponse> Atualizar(IEnumerable<AtualizarStatusAnuncioRequest> atualizacoes)
        {
            List<AtualizarStatusAnuncioResponse> responses = new();
            foreach (AtualizarStatusAnuncioRequest request in atualizacoes)
            {
                responses.Add(_mediator.Send(request).Result);
            }

            return responses;
        }
    }
}
