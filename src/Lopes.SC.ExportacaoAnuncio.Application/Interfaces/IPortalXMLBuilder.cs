using Lopes.SC.ExportacaoAnuncio.Application.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Application.Interfaces
{
    public interface IPortalXMLBuilder
    {
        Portal Portal { get; }
        /// <summary>
        /// Caminho do arquivo XML
        /// </summary>
        string CaminhoArquivo { get; }
        /// <summary>
        /// Insere ou atualiza as informações do imóvel no XML
        /// </summary>
        /// <param name="dados"></param>
        void InserirAtualizarImovel(Imovel dados);
        /// <summary>
        /// Remove o imóvel do XML
        /// </summary>
        /// <param name="idImovel"></param>
        void RemoverImovel(int idImovel);
        /// <summary>
        /// True se imóvel estiver no XML
        /// </summary>
        /// <param name="idImovel"></param>
        /// <returns></returns>
        bool ImovelNoXml(int idImovel);
        /// <summary>
        /// Retorna o Id dos imóveis no XML
        /// </summary>
        /// <param name="caminhoArquivo"></param>
        /// <returns></returns>
        IEnumerable<int> ObterIdImoveisNoXml();
    }
}
