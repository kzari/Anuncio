using Lopes.Acesso.Domain.Models;

namespace Lopes.Acesso.Domain.Services
{
    public interface IUsuarioDadosService
    {
        Usuario? ObterUsuario(string login);
    }
}