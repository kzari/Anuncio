using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.Models.XML;
using Lopes.Anuncio.Domain.Models.XML.Portais;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.XML;
using System.Text.RegularExpressions;

namespace Lopes.Anuncio.Domain.Services
{
    public abstract class PortalXmlElementosBase
    {
        protected readonly string UrlFotosProdutos;
        protected readonly IEnumerable<PortalCaracteristica> PortalCaracteristicas;

        public PortalXmlElementosBase(Portal portal, IEnumerable<PortalCaracteristica> portalCaracteristicas, string urlFotosProdutos)
        {
            Portal = portal;
            PortalCaracteristicas = portalCaracteristicas ?? new List<PortalCaracteristica>();
            UrlFotosProdutos = urlFotosProdutos;
        }

        public Portal Portal { get; }

        protected abstract Elemento CriarElementoCabecalho();
        protected abstract ElementoProduto CriarElementoProduto(Produto dados);

        public Xml ObterXml(Produto dados) => ObterXml(new List<Produto> { dados });
        public Xml ObterXml(IEnumerable<Produto> produtos)
        {
            Elemento cabecalhos = CriarElementoCabecalho();

            IEnumerable<ElementoProduto> eProdutos = CriarElementoProduto(produtos);

            return new Xml(cabecalhos, eProdutos);
        }
        public static IPortalXMLElementos ObterPortalXml(Portal portal, IEnumerable<PortalCaracteristica> portalCaracteristicas, string urlFotosProdutos)
        {
            switch (portal)
            {
                case Portal.Zap:
                default:
                    return new Zap(portal, portalCaracteristicas, urlFotosProdutos);
                //default:
                    //throw new NotImplementedException();
            }
        }

        protected static bool UsaZonaDeValor(string? estado)
        {
            if (string.IsNullOrEmpty(estado))
                return false;

            string[] estados = new [] { "SP", "RS", "RJ" };
            return estados.Any(a => a.Equals(estado));
        }

        public static string FormatarDecimal(decimal? valor) => valor.HasValue
                ? string.Format("{0:0.00}", valor.Value)
                : string.Empty;

        public static string RemoverCaracteresInvalidosUnicode(string? input, string replacement)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Regex.Replace(input, "\\p{C}+", replacement);
        }

        

        private IEnumerable<ElementoProduto> CriarElementoProduto(IEnumerable<Produto> produtos)
        {
            foreach (Produto imovel in produtos)
            {
                yield return CriarElementoProduto(imovel);
            }
        }
    }
}