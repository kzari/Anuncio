using Lopes.Anuncio.Application.Models;

namespace Lopes.Anuncio.Application.Services
{
    public interface ICotaAppService
    {
        IEnumerable<CotaResumoViewModel> ObterCotas();
    }
}
