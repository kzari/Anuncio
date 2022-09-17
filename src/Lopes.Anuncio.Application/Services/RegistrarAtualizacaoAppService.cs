using Lopes.Anuncio.Application.Interfaces.DadosService;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using MediatR;

namespace Lopes.Anuncio.Application.Services
{
    public class RegistrarAtualizacaoAppService : IRegistrarAtualizacaoAnunciosAppService
    {
        private readonly IMediator _mediator;

        public RegistrarAtualizacaoAppService(IMediator handler)
        {
            _mediator = handler;
        }


        public Task<AtualizarStatusAnuncioResponse> Registrar(RegistroAtualizacaoCommand request)
        {
            return _mediator.Send(request);
        }


        public IEnumerable<AtualizarStatusAnuncioResponse> Registrar(IEnumerable<RegistroAtualizacaoCommand> atualizacoes)
        {
            List<AtualizarStatusAnuncioResponse> responses = new();
            foreach (RegistroAtualizacaoCommand request in atualizacoes)
            {
                responses.Add(_mediator.Send(request).Result);
            }

            return responses;
        }
    }
}
