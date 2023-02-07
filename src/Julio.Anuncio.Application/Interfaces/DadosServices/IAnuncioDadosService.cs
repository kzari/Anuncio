using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.ObjetosValor;

namespace Julio.Anuncio.Application.DadosService
{
    public interface IAnuncioDadosService
    {
        IEnumerable<AnuncioCota> Obter(AnuncioCotaRequest request);
    }
}
