using Lopes.Acesso.Domain.Commands.Requests;
using Lopes.Acesso.Domain.Models;

namespace Lopes.Acesso.Domain.Services
{
    public interface IUsuarioAcessoService
    {
        Usuario ObterUsuario(LoginCommand loginRequest);
    }
}
