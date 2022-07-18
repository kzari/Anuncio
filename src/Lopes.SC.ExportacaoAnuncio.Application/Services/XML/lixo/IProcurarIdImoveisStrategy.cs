using Lopes.SC.ExportacaoAnuncio.Domain.Models;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public interface IProcurarIdImoveisStrategy
    {
        /// <summary>
        /// Retorna o Id dos Imóveis no portal
        /// </summary>
        /// <param name="caminhoArquivo"></param>
        /// <returns></returns>
        IEnumerable<int> ObterIdImoveis(string caminhoArquivo);

        /// <summary>
        /// True se imóvel está no portal
        /// </summary>
        /// <param name="caminhoArquivo"></param>
        /// <param name="idImovel"></param>
        /// <returns></returns>
        bool ImovelNoPortal(string caminhoArquivo, int idImovel);
    }
}
