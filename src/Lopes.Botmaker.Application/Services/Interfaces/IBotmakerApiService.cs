using Lopes.Botmaker.Application.Models;
using Lopes.Domain.Commons;

namespace Lopes.Botmaker.Application.Services
{
    public interface IBotmakerApiService
    {
        string Token { get; }

        IResultado<IEnumerable<UsuarioBotmakerApi>> ObterUsuariosNaBotmaker();
    }
}