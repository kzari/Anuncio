using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using System.Xml.XPath;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public class ProcurarIdImoveisStrategy : IProcurarIdImoveisStrategy
    {
        protected virtual string Query => "//Imoveis/Imovel/CodigoImovel";
        protected virtual string QueryImovelNoPortal(int idImovel) => $"//*[text() = 'REO{idImovel}']";

        public static IProcurarIdImoveisStrategy ObterEstrategia(Portal portal)
        {
            switch (portal)
            {
                case Portal.ChavesNaMao:
                    return new ProcurarIdImoveisVivaRealStrategy();

                case Portal.OlhoMagico:
                case Portal.MercadoLivre:
                case Portal.CliqueiMudei:
                    return new ProcurarIdImoveisVivaRealEnStrategy();

                case Portal.ChaveFacil:
                    return new ProcurarIdImoveisChaveFacilStrategy();

                case Portal.LuxuryEstate:
                    return new ProcurarIdImoveisLuxuryStateStrategy();

                default:
                    return new ProcurarIdImoveisStrategy();
            }
        }

        public virtual IEnumerable<int> ObterIdImoveis(string caminhoArquivo)
        {
            using Stream fileStream = File.Open(caminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(Query);
            XPathNodeIterator nodeIterator = navigator.Select(xPathExpression);

            while (nodeIterator.MoveNext())
            {
                string codigoImovel = nodeIterator.Current.Value;
                if (!string.IsNullOrEmpty(codigoImovel))
                {
                    codigoImovel = codigoImovel.Replace("REO", "");
                    if (int.TryParse(codigoImovel, out int idImovelInt))
                    {
                        yield return idImovelInt;
                    }
                }
            }
        }

        public bool ImovelNoPortal(string caminhoArquivo, int idImovel)
        {
            using Stream fileStream = File.Open(caminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(QueryImovelNoPortal(idImovel));
            XPathNavigator node = navigator.SelectSingleNode(xPathExpression);
            if (node == null)
                return false;

            string codigoImovel = node.Value;
            if (string.IsNullOrEmpty(codigoImovel))
                return false;
            
            codigoImovel = codigoImovel.Replace("REO", "");
            if (int.TryParse(codigoImovel, out int idImovelInt))
                return idImovelInt == idImovel;

            return false;
        }
    }
}
