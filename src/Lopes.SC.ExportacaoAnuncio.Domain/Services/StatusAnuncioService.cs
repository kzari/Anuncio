using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Reposities;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
{
    public class StatusAnuncioService : IStatusAnuncioService
    {
        private readonly IImovelRepository _imovelRepository;

        public StatusAnuncioService(IImovelRepository imovelRepository)
        {
            _imovelRepository = imovelRepository;
        }


        public StatusAnuncioPortal VerificarStatusImovelPortal(Anuncio anuncio, bool imovelNoXml)
        {
            
        }
    }
}
