using Lopes.Acesso.App.Models;

namespace Lopes.Acesso.App.Services
{
    public interface ITokenService
    {
        string Gerar(Usuario usuario);
    }
}
