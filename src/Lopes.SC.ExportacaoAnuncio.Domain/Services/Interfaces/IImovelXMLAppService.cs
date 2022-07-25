using Lopes.SC.Domain.Commons;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface IImovelXMLAppService
    {
        /// <summary>
        /// Caminho para a pasta dos arquivos XMLs
        /// </summary>
        string CaminhoPastaArquivos { get; }
        ///// <summary>
        ///// Retorna os Imóveis nos XMLs
        ///// </summary>
        ///// <returns></returns>
        //IEnumerable<PortalImoveisXML> ObterImoveisXMLs();
        ///// <summary>
        ///// Retorna os portais que o imóvel está 
        ///// </summary>
        ///// <param name="idImovel"></param>
        ///// <returns></returns>
        //IEnumerable<Portal> ObterPortaisImovel(int idImovel);
        /// <summary>
        /// Retorna o nome do arquivo XMl
        /// </summary>
        /// <param name="portal"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        IRetorno<string> CaminhoArquivoXml(Portal portal, int idEmpresa);
    }
}