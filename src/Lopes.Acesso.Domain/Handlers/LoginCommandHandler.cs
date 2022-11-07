using Lopes.Acesso.Domain.Commands.Requests;
using Lopes.Acesso.Domain.Commands.Responses;
using Lopes.Acesso.Domain.Models;
using Lopes.Acesso.Domain.Services;
using MediatR;

namespace Lopes.Acesso.Domain.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUsuarioDadosService _usuarioDadosService;

        public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Usuario? usuario = _usuarioDadosService.ObterUsuario(request.Login);
            if (usuario == null)
            {
                //TODO: Retornar erro
            }

            //TODO: Verificar se senha está correta


            //TODO: gerar o token

            return Task.FromResult(new LoginResponse("asd asçdl"));
        }
    }
}
