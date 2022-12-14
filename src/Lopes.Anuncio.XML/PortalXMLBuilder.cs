using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Models.XML;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Services;
using Lopes.Anuncio.Domain.XML;
using Lopes.Domain.Commons;
using Lopes.Acesso.Commons.Extensions;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Lopes.Acesso.XML
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
                                string urlFotosProdutos)
        {
            _idEmpresa = idEmpresa;
            _portal = portal;
            _caminhoArquivoPasta = caminhoArquivoPasta;
            _portalXmlElementos = PortalXmlElementosBase.ObterPortalXml(portal, portalCaracteristicas, urlFotosProdutos);
            _apelidoEmpresa = apelidoEmpresa;
        }


        public string CaminhoArquivo => caminhoArquivo ??= CaminhoArquivoXml();


        public virtual void InserirAtualizarProdutos(IEnumerable<Produto> dados, bool removerSeExistir = false, IProgresso progresso = null)
        {
            Xml xml = _portalXmlElementos.ObterXml(dados);

            if (!File.Exists(CaminhoArquivo))
                CriarXml("1.0", "UTF-8", xml.Cabecalhos, CaminhoArquivo);

            XmlDocument doc = new ();
            doc.Load(CaminhoArquivo);

            XmlNode? eProdutos = doc.SelectSingleNode(_portalXmlElementos.CaminhoTagPaiProdutos);
            if (eProdutos == null)
                throw new Exception($"Elemento não encontrado no XML. Caminho: '{_portalXmlElementos.CaminhoTagPaiProdutos}'.");

            progresso?.NovaMensagem("Montando elementos...");

            AdicionarProdutos(xml, doc, eProdutos, removerSeExistir, progresso);

            doc.Save(CaminhoArquivo);
        }

        private static void AdicionarProdutos(Xml xml, XmlDocument doc, XmlNode eProdutos, bool removerSeExistir, IProgresso? progresso = null)
        {
            List<ElementoProduto> elementos = xml.Produtos.ToList();

            int i = 0;
            int qtdeProdutos = elementos.Count;
            foreach (ElementoProduto eProduto in elementos)
            {
                i++;
                if (removerSeExistir)
                    RemoverProduto(doc, eProdutos, eProduto.IdProduto);

                AdicionarElemento(doc, eProdutos, eProduto);

                if (progresso != null && i % 100 == 0)
                    progresso.Mensagem($"Inserindo/atualizando no XML. {i} de {qtdeProdutos}", i);
            }
        }

        public void RemoverProdutos(int[] idProdutos, IProgresso? progresso = null)
        {
            if (idProdutos.Length == 0 || !File.Exists(CaminhoArquivo))
                return;

            XmlDocument doc = new();
            doc.Load(CaminhoArquivo);

            XmlNode? eProdutos = doc.SelectSingleNode(_portalXmlElementos.CaminhoTagPaiProdutos);
            if(eProdutos == null)
                throw new Exception($"Elemento não encontrado no XML. Caminho: '{_portalXmlElementos.CaminhoTagPaiProdutos}'.");

            int i = 0;
            foreach (int id in idProdutos)
            {
                i++;
                RemoverProduto(doc, eProdutos, id);

                progresso?.Mensagem($"{i} imóveis removido(s) no XML de {idProdutos.Length}.", i);
            }

            doc.Save(CaminhoArquivo);
        }

        private static void RemoverProduto(XmlDocument doc, XmlNode eProdutos, int idProduto)
        {
            string query = QueryIdProduto("REO" + idProduto);
            XmlNode? eProdutoExistente = doc.SelectSingleNode(query);
            if (eProdutoExistente?.ParentNode != null)
                eProdutos.RemoveChild(eProdutoExistente.ParentNode);
        }

        public bool ProdutoNoPortal(int idProduto)
        {
            if (!File.Exists(CaminhoArquivo))
                return false;

            using Stream fileStream = File.Open(CaminhoArquivo, FileMode.Open);
            XPathDocument xPath = new XPathDocument(fileStream);

            XPathNavigator navigator = xPath.CreateNavigator();
            XPathExpression xPathExpression = navigator.Compile(QueryIdProduto(idProduto));
            XPathNavigator node = navigator.SelectSingleNode(xPathExpression);
            return node != null;
        }

        private static void CriarXml(string versao, string codificacao, Elemento elemento, string caminhoArquivo)
        {
            XmlDocument doc = new ();
            XmlNode docNode = doc.CreateXmlDeclaration(versao, codificacao, null);
            doc.AppendChild(docNode);

            XmlElement? eRoot = CriarElemento(doc, elemento);
            if(eRoot != null)
            {
                doc.AppendChild(eRoot);
                if(elemento.Filhos != null)
                    AdicionarElementos(doc, eRoot, elemento.Filhos);
            }

            doc.Save(caminhoArquivo);
        }

        private static void AdicionarElemento(XmlDocument doc, XmlNode node, ElementoProduto elemento)
        {
            XmlElement? eProduto = CriarElemento(doc, elemento);
            if(eProduto != null)
                node.AppendChild(eProduto);
        }
        private static void AdicionarElementos(XmlDocument doc, XmlElement elementoPai, IEnumerable<Elemento> elementos)
        {
            foreach (var elemento in elementos)
                AdicionarElemento(doc, elementoPai, elemento);
        }
        private static void AdicionarElemento(XmlDocument doc, XmlElement elementoPai, Elemento elemento)
        {
            XmlElement? eCabecalho = CriarElemento(doc, elemento);
            if(eCabecalho != null)
                elementoPai.AppendChild(eCabecalho);
        }

        private static XmlElement? CriarElemento(XmlDocument doc, Elemento elemento)
        {
            if (elemento == null)
                return null;

            string nomeElemento = elemento.Nome.Replace(" ", "")
                                               .Replace("/", "")
                                               .Replace("\\", "")
                                               .Replace("'", "");

            XmlElement xmlElement = doc.CreateElement(nomeElemento);
            if (!string.IsNullOrEmpty(elemento.Valor))
                xmlElement.InnerText = elemento.Valor;

            AdicionarAtributos(elemento, xmlElement);

            AdicionarFilhos(doc, elemento, xmlElement);

            return xmlElement;
        }

        private static void AdicionarAtributos(Elemento elemento, XmlElement xmlElement)
        {
            if (elemento.Atributos.Algum())
            {
                foreach (Atributo atributo in elemento.Atributos)
                    xmlElement.SetAttribute(atributo.Nome, atributo.Valor);
            }
        }

        private static void AdicionarFilhos(XmlDocument doc, Elemento elemento, XmlElement xmlElement)
        {
            if (elemento.Filhos.Algum())
            {
                foreach (Elemento elementoFilho in elemento.Filhos)
                    AdicionarElemento(doc, xmlElement, elementoFilho);
            }
        }

        private static string QueryIdProduto(int idProduto) => QueryIdProduto("REO" + idProduto);
        private static string QueryIdProduto(string idProdutoPortal) => $"//*[text() = '{idProdutoPortal}']";

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

            return _caminhoArquivoPasta + "/" + nomePortal.ToLower() + "-" + _apelidoEmpresa + ".xml";
        }

        public IEnumerable<int> ObterIdProdutosNoPortal()
        {
            if (File.Exists(CaminhoArquivo))
            {
                XDocument xDocument = XDocument.Load(CaminhoArquivo);
                IEnumerable<string> idsProdutoString = xDocument.Descendants(_portalXmlElementos.NomeTagCodigoProduto)
                                                                .Select(_ => _.Value.Replace("REO", ""));
                foreach (string idString in idsProdutoString)
                {
                    if (int.TryParse(idString, out int idProduto))
                        yield return idProduto;
                }
            }
        }
    }
}
