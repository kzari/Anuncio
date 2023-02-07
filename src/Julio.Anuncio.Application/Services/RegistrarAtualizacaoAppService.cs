using Julio.Anuncio.Application.Interfaces.DadosService;
using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.Commands.Responses;
using Julio.Anuncio.Domain.Models.DadosProduto;
using MediatR;

namespace Julio.Anuncio.Application.Services
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
