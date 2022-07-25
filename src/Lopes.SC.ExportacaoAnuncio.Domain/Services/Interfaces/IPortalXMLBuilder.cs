using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
{
    public interface IPortalXMLBuilder
    {
        /// <summary>
        /// Insere ou atualiza as informações do imóvel no XML
        /// </summary>
        /// <param name="dados"></param>
        void InserirAtualizarImoveis(Xml dadosXml, string caminhoArquivo);
        /// <summary>
        /// Remove o imóvel do XML
        /// </summary>
        /// <param name="idImovel"></param>
        void RemoverImovel(int idImovel, string caminhoArquivo);
        /// <summary>
        /// True se imóvel estiver no XML
        /// </summary>
        /// <param name="idImovel"></param>
        /// <returns></returns>
        bool ImovelNoXml(int idImovel, string caminhoArquivo, string query = null);
        ///// <summary>
        ///// Retorna o Id dos imóveis no XML
        ///// </summary>
        ///// <param name="caminhoArquivo"></param>
        ///// <returns></returns>
        //IEnumerable<int> ObterIdImoveisNoXml(string caminhoArquivo);
    }
}
