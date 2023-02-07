using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.ObjetosValor;

namespace Julio.Anuncio.Application.DadosService
{
    public interface ICotaDadosService
    {
        IEnumerable<CotaResumo> Obter(CotaResumoRequest request);
    }
}
