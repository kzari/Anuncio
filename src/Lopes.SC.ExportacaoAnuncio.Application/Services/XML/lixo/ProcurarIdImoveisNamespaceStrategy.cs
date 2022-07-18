using System.Xml;

namespace Lopes.SC.ExportacaoAnuncio.Application.Services.XML
{
    public abstract class ProcurarIdImoveisNamespaceStrategy : ProcurarIdImoveisStrategy
    {
        protected abstract string Namespace { get; }
        public override IEnumerable<int> ObterIdImoveis(string caminhoArquivo)
        {
            XmlDocument document = new XmlDocument();
            document.Load(caminhoArquivo);
            XmlNamespaceManager m = new XmlNamespaceManager(document.NameTable);
            m.AddNamespace("ns", Namespace);
            XmlNodeList nodes = document.SelectNodes(Query, m);

            foreach (XmlElement node in nodes)
            {
                string codigoImovel = node.InnerText;
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
    }
}
