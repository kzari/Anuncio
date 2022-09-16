using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.DadosService
{
    public interface IFranquiaApelidoPortalDadosService
    {
        IEnumerable<FranquiaApelido> Obter();
    }
}
