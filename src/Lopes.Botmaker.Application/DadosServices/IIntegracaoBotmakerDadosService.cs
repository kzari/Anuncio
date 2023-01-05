using Lopes.Botmaker.Application.Models;

namespace Lopes.Botmaker.Application.DadosServices
{
    public interface IIntegracaoBotmakerDadosService
    {
        IEnumerable<DadosUsuarioDTO> ObterUsuariosIntegracao(params int[] idsEmpresas);
        IEnumerable<DadosUsuarioDTO> ObterUsuariosIntegracao(string[] emails, params int[] idsEmpresas);
    }
}
