using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
{
    public interface IStatusAnuncioService
    {
        StatusAnuncioPortal VerificarStatusImovelPortal(Anuncio anuncio, bool imovelNoXml);
    }
}
