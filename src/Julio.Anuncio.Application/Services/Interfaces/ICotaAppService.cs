using Julio.Anuncio.Application.Models;
using Julio.Anuncio.Domain.Enums;

namespace Julio.Anuncio.Application.Services
{
    public interface ICotaAppService
    {
        IEnumerable<CotaResumoViewModel> ObterCotas();
        AnunciosDesatualizadosViewModel ObterAnunciosDesatualizadosPorPortal(int portal);
        AnunciosDesatualizadosViewModel ObterAnunciosDesatualizadosPorCota(int idCota);
    }
}
