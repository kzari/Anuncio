using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models.XML;

namespace Lopes.Anuncio.Domain.Services
{
    public interface IPortalXMLElementos
    {
        string CaminhoTagPaiImoveis { get; }
        string NomeTagImovel { get; }
        string NomeTagCodigoImovel { get; }

        Xml ObterXml(IEnumerable<DadosImovel> imoveis);
        Xml ObterXml(DadosImovel dados);
    }
}