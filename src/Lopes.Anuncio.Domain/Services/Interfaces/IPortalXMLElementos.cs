using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Models.XML;

namespace Lopes.Anuncio.Domain.Services
{
    public interface IPortalXMLElementos
    {
        string CaminhoTagPaiProdutos { get; }
        string NomeTagImovel { get; }
        string NomeTagCodigoImovel { get; }

        Xml ObterXml(IEnumerable<Produto> imoveis);
        Xml ObterXml(Produto dados);
    }
}