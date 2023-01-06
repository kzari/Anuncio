using Lopes.Botmaker.Application.Models;
using Lopes.Domain.Commons;

namespace Lopes.Botmaker.Application.Services
{
    public interface IIntegracaoAppService
    {
        void IntegrarTudo(ILogger? logger = null);
        IEnumerable<UsuarioIntegracao> ObterUsuarios();
    }
}