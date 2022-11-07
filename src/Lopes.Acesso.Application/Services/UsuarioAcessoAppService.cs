using Lopes.Acesso.Domain.Commands.Requests;
using Lopes.Acesso.Domain.Commands.Responses;
using MediatR;

namespace Lopes.Acesso.Application.Services
{
    public class UsuarioAcessoAppService : IUsuarioAcessoAppService
    {
        private readonly IMediator _mediator;

        public UsuarioAcessoAppService(IMediator mediator)
        {
            _mediator = mediator;
        }


        public string ObterTokenAutenticado(string login, string senha)
        {
            LoginCommand command = new(login, senha);
            LoginResponse response = _mediator.Send(command).Result;
            
            return response.Token;
        }
    }
}