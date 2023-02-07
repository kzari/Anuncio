using Julio.Botmaker.Application.Models;

namespace Julio.Botmaker.Application.DadosServices
{
    public interface IIntegracaoBotmakerDadosService
    {
        IEnumerable<DadosUsuarioDTO> ObterUsuariosIntegracao(params int[] idsEmpresas);
        IEnumerable<DadosUsuarioDTO> ObterUsuariosIntegracao(string[] emails, params int[] idsEmpresas);
    }
}
