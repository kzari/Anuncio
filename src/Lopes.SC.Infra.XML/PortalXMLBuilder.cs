using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;
using Lopes.SC.ExportacaoAnuncio.Domain.XML;
using Lopes.SC.Domain.Commons;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Lopes.SC.Infra.XML
{
    public class PortalXMLBuilder : IPortalAtualizadorXml
    {
        private readonly IPortalXMLElementos _portalXmlElementos;
        private readonly Portal _portal;
        private readonly int _idEmpresa;
        private readonly string _caminhoArquivoPasta;
        private readonly string _apelidoEmpresa;

        private string caminhoArquivo;


        public PortalXMLBuilder(string caminhoArquivoPasta,
                                IEnumerable<PortalCaracteristica> portalCaracteristicas,
                                string apelidoEmpresa,
                                Portal portal,
                                int idEmpresa,
                                string urlFotosImoveis)
        {
            _idEmpresa = idEmpresa;
            _portal = portal;
            _caminhoArquivoPasta = caminhoArquivoPasta;
            _portalXmlElementos = PortalXmlElementosBase.ObterPortalXml(portal, portalCaracteristicas, urlFotosImoveis);
            _apelidoEmpresa = apelidoEmpresa;
        }


        public string CaminhoArquivo => caminhoArquivo ??= CaminhoArquivoXml();


        public virtual void InserirAtualizarImoveis(IEnumerable<DadosImovel> dados, bool removerSeExistir = false, IProgresso progresso = null)
        {
            Xml xml = _portalXmlElementos.ObterXml(dados);

            if (!File.Exists(CaminhoArquivo))
                CriarXml("1.0", "UTF-8", xml.Cabecalhos, CaminhoArquivo);

            XmlDocument doc = new XmlDocument();
            doc.Load(CaminhoArquivo);

            XmlNode? eImoveis = doc.SelectSingleNode(_portalXmlElementos.CaminhoTagPaiImoveis);

            if (progresso != null)
                progresso.Atualizar($"Montando elementos...");

            List<ElementoImovel> elementos = xml.Imoveis.ToList();

            int i = 0;
            int qtdeImoveis = elementos.Count;
            foreach (ElementoImovel eImovel in elementos)
            {
                i++;
                if (removerSeExistir)
                    RemoverImovel(doc, eImoveis, eImovel.IdImovel);
                
                AdicionarElemento(doc, eImoveis, eImovel);

                if(progresso != null)
                    progresso.Atualizar($"Inserindo/atualizando no XML. {i} de {qtdeImoveis}", i);
            }

            doc.Save(CaminhoArquivo);
        }

        public void RemoverImoveis(int[] idImoveis, IProgresso progresso = null)
        {
            if (!File.Exists(CaminhoArquivo))
                return;

            XmlDocument doc = new XmlDocument();
            doc.Load(CaminhoArquivo);

            XmlNode? eImoveis = doc.SelectSingleNode(_portalXmlElementos.CaminhoTagPaiImoveis);

            int i = 0;
            foreach (int id in idImoveis)
            {
                i++;
                RemoverImovel(doc, eImoveis, id);
                
                if (progresso != null)
                    progresso.Atualizar($"{i} imóveis removido(s) no XML de {idImoveis.Length}.", i);
            }

            doc.Save(CaminhoArquivo);
        }

        private static void RemoverImovel(XmlDocument doc, XmlNode eImoveis, int idImovel)
        {
            string query = QueryIdImovel("REO" + idImovel);
            XmlNode? eImovelExistente = doc.SelectSingleNode(query);
            if (eImovelExistente != null)
                eImoveis.RemoveChild(eImovelExistente.ParentNode);
        }

        public bool ImovelNoPortal(int idImovel)
        {
            if (!File.Exists(CaminhoArquivo))
                return false;

            using Stream fileStream = File.Open(CaminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(QueryIdImovel(idImovel));
            XPathNavigator node = navigator.SelectSingleNode(xPathExpression);
            if (node == null)
                return false;

            return true;
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

        private string CaminhoArquivoXml()
        {
            if (string.IsNullOrEmpty(_apelidoEmpresa))
                throw new Exception($"Apelido empresa não encontrado. Id Empresa: {_idEmpresa}");

            string nomePortal = Enum.GetName(_portal);
            if (string.IsNullOrEmpty(nomePortal))
                throw new Exception($"Portal não encontrado. Id portal: {_portal}");

            string caminhoArquivo = _caminhoArquivoPasta + "/" + nomePortal.ToLower() + "-" + _apelidoEmpresa + ".xml";

            return caminhoArquivo;
        }

        public IEnumerable<int> ObterIdImoveisNoPortal()
        {
            if (File.Exists(CaminhoArquivo))
            {
                XDocument xDocument = XDocument.Load(CaminhoArquivo);
                IEnumerable<string> idsImovelString = xDocument.Descendants(_portalXmlElementos.NomeTagCodigoImovel)
                                                               .Select(_ => _.Value.Replace("REO", ""));
                foreach (string idString in idsImovelString)
                {
                    if (int.TryParse(idString, out int idImovel))
                        yield return idImovel;
                }
            }
        }
    }
}
