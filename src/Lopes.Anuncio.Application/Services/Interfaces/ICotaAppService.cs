using Lopes.Anuncio.Application.Models;
using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Application.Services
{
    public interface ICotaAppService
    {
        IEnumerable<CotaResumoViewModel> ObterCotas();
        AnunciosDesatualizadosViewModel ObterAnunciosDesatualizados(int portal);
    }
}
