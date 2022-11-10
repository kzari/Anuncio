using Lopes.Acesso.App.Models;

namespace Lopes.Acesso.App.Services
{
    public interface IUsuarioAcessoAppService
    {
        LoginModel ObterTokenAutenticado(string login, string senha);
    }
}