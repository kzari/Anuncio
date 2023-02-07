using Julio.Botmaker.Application.Models;
using Julio.Domain.Commons;

namespace Julio.Botmaker.Application.Services
{
    public interface IBotmakerApiService
    {
        string Token { get; }

        IResultado AtualizarUsuarioChatbox(BotmakerInserirAtualizarUsuario item);
        IResultado<IEnumerable<UsuarioBotmakerApi>> ObterUsuariosNaBotmaker();
        IResultado RemoverUsuarioChatbox(string email);
    }
}