using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.ObjetosValor;

namespace Julio.Anuncio.Domain.Services
{
    public interface IStatusAnuncioService
    {
        StatusAnuncioPortal VerificarStatusProdutoPortal(AnuncioCota anuncio, bool imovelNoXml);
    }
}
