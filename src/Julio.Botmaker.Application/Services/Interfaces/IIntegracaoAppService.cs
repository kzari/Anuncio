using Julio.Botmaker.Application.Models;
using Julio.Domain.Commons;

namespace Julio.Botmaker.Application.Services
{
    public interface IIntegracaoAppService
    {
        IResultadoItens EnviarUsuarios(string[] emails);
        IResultadoItens RemoverUsuarios(string[] emails);
        IResultadoItens IntegrarUsuarios(ILogger? logger = null);
        IEnumerable<UsuarioIntegracao> ObterUsuarios();
        IEnumerable<UsuarioIntegracao> ObterUsuarios(TimeSpan duracaoCacheBotmaker, TimeSpan duracaoCacheBd);
        IResultadoItens InserirAtualizarUsuarios(IEnumerable<DadosUsuarioDTO> usuarios, bool removerAntesDeIncluir);
        IResultado RemoverUsuario(string email);
    }
}