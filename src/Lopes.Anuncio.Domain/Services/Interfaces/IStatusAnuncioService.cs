using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Domain.Services
{
    public interface IStatusAnuncioService
    {
        StatusAnuncioPortal VerificarStatusImovelPortal(Models.AnuncioImovel anuncio, bool imovelNoXml);
    }
}
