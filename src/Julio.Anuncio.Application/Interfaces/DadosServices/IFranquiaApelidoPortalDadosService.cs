using Julio.Anuncio.Domain.ObjetosValor;

namespace Julio.Anuncio.Application.DadosService
{
    public interface IFranquiaApelidoPortalDadosService
    {
        IEnumerable<FranquiaApelido> Obter();
    }
}
