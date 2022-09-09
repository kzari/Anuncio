using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;
using MediatR;

namespace Lopes.Anuncio.Domain.Handlers
{
    public class AtualizarStatusAnuncioHandlers : IRequestHandler<AtualizarStatusAnuncioRequest, AtualizarStatusAnuncioResponse>
    {
        private readonly IAnuncioStatusRepositorioGravacao _repository;

        public AtualizarStatusAnuncioHandlers(IAnuncioStatusRepositorioGravacao repository)
        {
            _repository = repository;
        }


        public Task<AtualizarStatusAnuncioResponse> Handle(AtualizarStatusAnuncioRequest request, CancellationToken cancellationToken)
        {
            _repository.AtualizarOuAdicionar(request);

            return Task.FromResult(new AtualizarStatusAnuncioResponse(request));
        }
    }
}
