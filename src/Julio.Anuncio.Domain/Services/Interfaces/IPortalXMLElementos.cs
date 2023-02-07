using Julio.Anuncio.Domain.Models.DadosProduto;
using Julio.Anuncio.Domain.Models.XML;

namespace Julio.Anuncio.Domain.Services
{
    public interface IPortalXMLElementos
    {
        string CaminhoTagPaiProdutos { get; }
        string NomeTagProduto { get; }
        string NomeTagCodigoProduto { get; }

        Xml ObterXml(IEnumerable<Produto> produtos);
        Xml ObterXml(Produto dados);
    }
}