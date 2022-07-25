namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    //public abstract class PortalXMLBuilder
    //{
    //    protected PortalXMLBuilder(Portal portal, string caminhoArquivo)
    //    {
    //        Portal = portal;
    //        CaminhoArquivo = caminhoArquivo;
    //    }

    //    public Portal Portal { get; }
    //    public string CaminhoArquivo { get; }

    //    public static IPortalXMLBuilder ObterXmlBuilder(Portal portal, string caminhoArquivo)
    //    {
    //        switch (portal)
    //        {
    //            case Portal.Zap:
    //                return new ZapXMLBuilder(portal, caminhoArquivo);
    //            default:
    //                return new ZapXMLBuilder(portal, caminhoArquivo);
    //        }
    //    }

    //    protected IEnumerable<int> ObterIdImoveisXml(string @namespace, string query)
    //    {
    //        XmlDocument document = new XmlDocument();
    //        document.Load(CaminhoArquivo);

    //        XmlNamespaceManager m = new XmlNamespaceManager(document.NameTable);
    //        m.AddNamespace("ns", @namespace);

    //        XmlNodeList nodes = document.SelectNodes(query, m);

    //        foreach (XmlElement node in nodes)
    //        {
    //            string codigoImovel = node.InnerText;
    //            if (!string.IsNullOrEmpty(codigoImovel))
    //            {
    //                codigoImovel = codigoImovel.Replace("REO", "");
    //                if (int.TryParse(codigoImovel, out int idImovelInt))
    //                {
    //                    yield return idImovelInt;
    //                }
    //            }
    //        }
    //    }

    //    protected IEnumerable<int> ObterIdImoveisNoXml(string query)
    //    {
    //        using Stream fileStream = File.Open(CaminhoArquivo, FileMode.Open);
    //        XPathDocument xPath = new XPathDocument(fileStream);

    //        XPathNavigator navigator = xPath.CreateNavigator();
    //        XPathExpression xPathExpression = navigator.Compile(query);
    //        XPathNodeIterator nodeIterator = navigator.Select(xPathExpression);

    //        while (nodeIterator.MoveNext())
    //        {
    //            string codigoImovel = nodeIterator.Current.Value;
    //            if (!string.IsNullOrEmpty(codigoImovel))
    //            {
    //                codigoImovel = codigoImovel.Replace("REO", "");
    //                if (int.TryParse(codigoImovel, out int idImovelInt))
    //                {
    //                    yield return idImovelInt;
    //                }
    //            }
    //        }
    //    }

    //    public bool ImovelNoXml(int idImovel) => ImovelNoXml(idImovel, null);
    //    public bool ImovelNoXml(int idImovel, string query)
    //    {
    //        if (!File.Exists(CaminhoArquivo))
    //            return false;

    //        using Stream fileStream = File.Open(CaminhoArquivo, FileMode.Open);
    //        XPathDocument xPath = new XPathDocument(fileStream);

    //        XPathNavigator navigator = xPath.CreateNavigator();
    //        XPathExpression xPathExpression = navigator.Compile(query ?? $"//*[text() = 'REO{idImovel}']");
    //        XPathNavigator node = navigator.SelectSingleNode(xPathExpression);
    //        if (node == null)
    //            return false;

    //        string codigoImovel = node.Value;
    //        if (string.IsNullOrEmpty(codigoImovel))
    //            return false;

    //        codigoImovel = codigoImovel.Replace("REO", "");
    //        if (int.TryParse(codigoImovel, out int idImovelInt))
    //            return idImovelInt == idImovel;

    //        return false;
    //    }

    //    protected static string SubstituirCaracteresInvalidosUnicode(string input, string substituto = "")
    //    {
    //        return Regex.Replace(input, "\\p{C}+", substituto);
    //    }
    //}
}
