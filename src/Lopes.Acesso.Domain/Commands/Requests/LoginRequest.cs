using Lopes.Acesso.Domain.Commands.Responses;
using MediatR;

namespace Lopes.Acesso.Domain.Commands.Requests
{
    public class LoginCommand : IRequest<LoginResponse>
    {

        public LoginCommand(string login, string senha)
        {
            Login = login;
            Senha = senha;
        }

        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
