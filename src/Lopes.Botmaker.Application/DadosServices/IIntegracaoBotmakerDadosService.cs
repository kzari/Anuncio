using Lopes.Botmaker.Application.Models;

namespace Lopes.Botmaker.Application.DadosServices
{
    public interface IIntegracaoBotmakerDadosService
    {
        IEnumerable<UsuarioIntegracaoBotmakerDTO> ObterUsuariosIntegracao(params int[] idsEmpresas);
        IEnumerable<UsuarioIntegracaoBotmakerDTO> ObterUsuariosIntegracao(string[] emails, params int[] idsEmpresas);
    }
}
