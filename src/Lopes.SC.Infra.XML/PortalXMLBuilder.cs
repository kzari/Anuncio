using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.ExportacaoAnuncio.Domain.XML;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace Lopes.SC.Infra.XML
{
    public class PortalXMLBuilder : IPortalXMLBuilder
    {
        public virtual void InserirAtualizarImoveis(Xml xml, string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
                CriarXml("1.0", "UTF-8", xml.Cabecalhos, caminhoArquivo);

            XmlDocument doc = new XmlDocument();
            doc.Load(caminhoArquivo);

            XmlNode? eImoveis = doc.SelectSingleNode(xml.CaminhoTagPaiImoveis);


            foreach (ElementoImovel eImovel in xml.Imoveis)
            {
                //Removendo se existir
                string query = QueryIdImovel("REO" + eImovel.IdImovel);
                XmlNode? eImovelExistente = doc.SelectSingleNode(query);
                if (eImovelExistente != null)
                    eImoveis.RemoveChild(eImovelExistente.ParentNode);

                AdicionarElemento(doc, eImoveis, eImovel);
            }

            doc.Save(caminhoArquivo);
        }

        //public bool ImovelNoXml(int idImovel) => ImovelNoXml(idImovel, null);
        public bool ImovelNoXml(int idImovel, string caminhoArquivo, string query = null)
        {
            if (!File.Exists(caminhoArquivo))
                return false;

            using Stream fileStream = File.Open(caminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(query ?? QueryIdImovel(idImovel));
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
        public IEnumerable<int> ObterIdImoveisXml(string @namespace, string query, string caminhoArquivo)
        {
            XmlDocument document = new XmlDocument();
            document.Load(caminhoArquivo);

            XmlNamespaceManager m = new XmlNamespaceManager(document.NameTable);
            m.AddNamespace("ns", @namespace);

            XmlNodeList nodes = document.SelectNodes(query, m);

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
        public IEnumerable<int> ObterIdImoveisNoXml(string query, string caminhoArquivo)
        {
            using Stream fileStream = File.Open(caminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(query);
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

        private void CriarXml(string versao, string codificacao, Elemento elemento, string caminhoArquivo)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration(versao, codificacao, null);
            doc.AppendChild(docNode);

            XmlElement eRoot = CriarElemento(doc, elemento);
            doc.AppendChild(eRoot);

            AdicionarElementos(doc, eRoot, elemento.Filhos);

            doc.Save(caminhoArquivo);
        }

        private static void AdicionarElemento(XmlDocument doc, XmlNode node, ElementoImovel elemento)
        {
            XmlElement eImovel = CriarElemento(doc, elemento);
            node.AppendChild(eImovel);
        }
        private static void AdicionarElementos(XmlDocument doc, XmlElement elementoPai, IEnumerable<Elemento> elementos)
        {
            foreach (var elemento in elementos)
                AdicionarElemento(doc, elementoPai, elemento);
        }
        private static void AdicionarElemento(XmlDocument doc, XmlElement elementoPai, Elemento elemento)
        {
            XmlElement eCabecalho = CriarElemento(doc, elemento);
            elementoPai.AppendChild(eCabecalho);
        }
        
        private static XmlElement CriarElemento(XmlDocument doc, Elemento elemento)
        {
            if (elemento == null)
                return null;

            XmlElement xmlElement = doc.CreateElement(elemento.Nome);
            if (!string.IsNullOrEmpty(elemento.Valor))
                xmlElement.InnerText = elemento.Valor;

            if (elemento.Atributos.Any())
                foreach (Atributo atributo in elemento.Atributos)
                    (xmlElement).SetAttribute(atributo.Nome, atributo.Valor);

            //Adicionando filhos
            foreach (Elemento elementoFilho in elemento.Filhos)
                AdicionarElemento(doc, xmlElement, elementoFilho);

            return xmlElement;
        }
        
        private static string QueryIdImovel(int idImovel) => QueryIdImovel("REO" + idImovel);
        private static string QueryIdImovel(string idImovelPortal) => $"//*[text() = '{idImovelPortal}']";

        protected static string SubstituirCaracteresInvalidosUnicode(string input, string substituto = "")
        {
            return Regex.Replace(input, "\\p{C}+", substituto);
        }

        public void RemoverImovel(int idImovel, string caminhoArquivo)
        {
            throw new NotImplementedException();
        }
    }
}
