using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
{
    public interface IStatusAnuncioService
    {
        /// <summary>
        /// Retorna o status do Imóvel/Anúncio no portal
        /// </summary>
        /// <param name="anuncio"></param>
        /// <param name="portaisInseridos">Portais onde o imóvel foi inserido</param>
        /// <returns></returns>
        StatusImovelPortal VerificarStatusImovelPortal(Anuncio anuncio, params Portal[] portaisInseridos);
        /// <summary>
        /// Retorna o status do Imóvel/Anúncio no portal
        /// </summary>
        /// <param name="anuncio"></param>
        /// <param name="imovelNoXml">True se o imóvel estiver no XML</param>
        /// <returns></returns>
        StatusImovelPortal VerificarStatusImovelPortal(Anuncio anuncio, bool imovelNoXml);
    }
}