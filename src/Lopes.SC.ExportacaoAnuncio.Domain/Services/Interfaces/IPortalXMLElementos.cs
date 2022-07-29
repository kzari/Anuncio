using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
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