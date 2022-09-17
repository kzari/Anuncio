using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Domain.Services
{
    public interface IStatusAnuncioService
    {
        StatusAnuncioPortal VerificarStatusProdutoPortal(AnuncioCota anuncio, bool imovelNoXml);
    }
}
