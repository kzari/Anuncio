using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.DadosService
{
    public interface IAnuncioDadosAppService
    {
        IEnumerable<AnuncioCota> Obter(AnuncioCotaRequest request);
    }
}
