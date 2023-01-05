using Lopes.Botmaker.Application.Models;

namespace Lopes.Botmaker.Application.Services
{
    public interface IIntegracaoAppService
    {
        void IntegrarTudo();
        IEnumerable<UsuarioIntegracao> ObterUsuarios();
    }
}