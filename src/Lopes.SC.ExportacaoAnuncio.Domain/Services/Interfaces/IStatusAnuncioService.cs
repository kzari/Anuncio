using Lopes.SC.Anuncio.Domain.Enums;
using Lopes.SC.Anuncio.Domain.Models;

namespace Lopes.SC.Anuncio.Domain.Services
{
    public interface IStatusAnuncioService
    {
        StatusAnuncioPortal VerificarStatusImovelPortal(Models.AnuncioImovel anuncio, bool imovelNoXml);
    }
}
