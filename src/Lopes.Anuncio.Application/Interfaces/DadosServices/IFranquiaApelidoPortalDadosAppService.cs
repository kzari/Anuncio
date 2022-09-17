using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.DadosService
{
    public interface IFranquiaApelidoPortalDadosAppService
    {
        IEnumerable<FranquiaApelido> Obter();
    }
}
