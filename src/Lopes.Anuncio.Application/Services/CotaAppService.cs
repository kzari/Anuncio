using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Application.Models;
using Lopes.Anuncio.Domain.Commands.Requests;

namespace Lopes.Anuncio.Application.Services
{
    public class CotaAppService : ICotaAppService
    {
        private readonly ICotaDadosService _dadosService;

        public CotaAppService(ICotaDadosService dadosService)
        {
            _dadosService = dadosService;
        }

        public IEnumerable<CotaResumoViewModel> ObterCotas()
        {
            return _dadosService.Obter(new CotaResumoRequest()).ToList().Select(_ => new CotaResumoViewModel(_));
        }
    }
}
