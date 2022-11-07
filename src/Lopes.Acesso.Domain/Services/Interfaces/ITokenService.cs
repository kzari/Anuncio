using Lopes.Acesso.Domain.Models;

namespace Lopes.Acesso.Domain.Services
{
    public interface ITokenService
    {
        string Gerar(Usuario usuario);
    }
}
