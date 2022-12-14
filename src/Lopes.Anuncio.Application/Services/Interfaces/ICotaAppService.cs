using Lopes.Anuncio.Application.Models;
using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Application.Services
{
    public interface ICotaAppService
    {
        IEnumerable<CotaResumoViewModel> ObterCotas();
        AnunciosDesatualizadosViewModel ObterAnunciosDesatualizadosPorPortal(int portal);
        AnunciosDesatualizadosViewModel ObterAnunciosDesatualizadosPorCota(int idCota);
    }
}
