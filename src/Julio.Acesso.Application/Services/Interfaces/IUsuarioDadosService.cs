using Julio.Acesso.App.Models;

namespace Julio.Acesso.App.Services
{
    public interface IUsuarioDadosService
    {
        Usuario? ObterUsuario(string login);
    }
}