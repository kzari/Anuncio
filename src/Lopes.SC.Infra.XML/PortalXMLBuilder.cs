using Lopes.SC.AnuncioXML.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace Lopes.SC.Infra.XML
{
    public class PortalXMLBuilder
    {
        public PortalXMLBuilder(string caminhoArquivo)
        {
            CaminhoArquivo = caminhoArquivo;
        }

        public string CaminhoArquivo { get; }

        public virtual void InserirAtualizarImovel(int idImovel, Elemento cabecalhos, Elemento imovel)
        {
            if (!File.Exists(CaminhoArquivo))
                CriarXml("1.0", "UTF-8", cabecalhos);

            XmlDocument doc = new XmlDocument();
            doc.Load(CaminhoArquivo);

            XmlNode? eImoveis = doc.SelectSingleNode("/Carga/Imoveis");

            //Removendo se existir
            XmlNode? eImovelExistente = doc.SelectSingleNode(QueryIdImovel("REO" + idImovel));
            if (eImovelExistente != null)
                eImoveis.RemoveChild(eImovelExistente.ParentNode);

            AdicionarElemento(doc, eImoveis, imovel);

            doc.Save(CaminhoArquivo);
        }

        public bool ImovelNoXml(int idImovel) => ImovelNoXml(idImovel, null);
        public bool ImovelNoXml(int idImovel, string query)
        {
            if (!File.Exists(CaminhoArquivo))
                return false;

            using Stream fileStream = File.Open(CaminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(query ?? $"//*[text() = 'REO{idImovel}']");
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
        public IEnumerable<int> ObterIdImoveisXml(string @namespace, string query)
        {
            XmlDocument document = new XmlDocument();
            document.Load(CaminhoArquivo);

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
        public IEnumerable<int> ObterIdImoveisNoXml(string query)
        {
            using Stream fileStream = File.Open(CaminhoArquivo, FileMode.Open);
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

        private void CriarXml(string versao, string codificacao, Elemento elemento)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration(versao, codificacao, null);
            doc.AppendChild(docNode);

            XmlElement eRoot = CriarElemento(doc, elemento);
            doc.AppendChild(eRoot);

            AdicionarElementos(doc, eRoot, elemento.Filhos);

            doc.Save(CaminhoArquivo);
        }

        private static void AdicionarElemento(XmlDocument doc, XmlNode node, Elemento elemento)
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
        private static string QueryIdImovel(string idImovelPortal) => $"Carga/Imoveis/Imovel/CodigoImovel[text()='{idImovelPortal}']";

        protected static string SubstituirCaracteresInvalidosUnicode(string input, string substituto = "")
        {
            return Regex.Replace(input, "\\p{C}+", substituto);
        }
    }
}
