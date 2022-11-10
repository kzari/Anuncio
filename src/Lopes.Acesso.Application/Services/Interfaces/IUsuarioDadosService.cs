using Lopes.Acesso.App.Models;

namespace Lopes.Acesso.App.Services
{
    public interface IUsuarioDadosService
    {
        Usuario? ObterUsuario(string login);
    }
}