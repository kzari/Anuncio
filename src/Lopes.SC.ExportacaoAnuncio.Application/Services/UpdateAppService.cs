using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.Application.Models;


namespace Lopes.SC.ExportacaoAnuncio.Application.Services
{
    public class UpdateAppService
    {
        private readonly ILogger _logger;
        private readonly IAnuncioAppService _anuncioAppService;
        private readonly IServiceProvider _serviceProvider;

        private readonly List<Imovel> _dadosImoveisCache;

        public UpdateAppService()
        {
        }

        public void Atualizar()
        {

        }
    }
}