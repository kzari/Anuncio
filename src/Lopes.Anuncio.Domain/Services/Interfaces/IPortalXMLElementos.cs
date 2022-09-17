using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Models.XML;

namespace Lopes.Anuncio.Domain.Services
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