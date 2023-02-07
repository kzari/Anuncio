using Julio.Acesso.App.Models;

namespace Julio.Acesso.App.Services
{
    public interface ITokenService
    {
        string Gerar(Usuario usuario);
    }
}
