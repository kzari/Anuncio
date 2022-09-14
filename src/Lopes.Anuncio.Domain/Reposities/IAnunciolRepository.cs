using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Domain.Reposities
{
    public interface IAnuncioRepository
    {
        IEnumerable<AnuncioCota> Obter(AnuncioCotaRequest request);
    }
}
