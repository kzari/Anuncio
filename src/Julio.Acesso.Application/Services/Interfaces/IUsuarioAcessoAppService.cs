using Julio.Acesso.App.Models;

namespace Julio.Acesso.App.Services
{
    public interface IUsuarioAcessoAppService
    {
        LoginModel ObterTokenAutenticado(string login, string senha);
    }
}