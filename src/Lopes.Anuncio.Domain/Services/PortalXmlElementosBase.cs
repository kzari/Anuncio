using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Models.XML;
using Lopes.Anuncio.Domain.XML;
using System.Text.RegularExpressions;

namespace Lopes.Anuncio.Domain.Services
{
    public abstract class PortalXmlElementosBase
    {

        public PortalXmlElementosBase(Portal portal, IEnumerable<PortalCaracteristica> portalCaracteristicas, string urlFotosImoveis)
        {
            Portal = portal;
            PortalCaracteristicas = portalCaracteristicas ?? new List<PortalCaracteristica>();
            UrlFotosImoveis = urlFotosImoveis;
        }

        public Portal Portal { get; }

        protected readonly string UrlFotosImoveis;
        protected readonly IEnumerable<PortalCaracteristica> PortalCaracteristicas;
        protected abstract Elemento CriarElementoCabecalho();
        protected abstract ElementoImovel CriarElementoImovel(DadosImovel dados);

        public Xml ObterXml(DadosImovel dados) => ObterXml(new List<DadosImovel> { dados });
        public Xml ObterXml(IEnumerable<DadosImovel> imoveis)
        {
            Elemento cabecalhos = CriarElementoCabecalho();

            IEnumerable<ElementoImovel> eImoveis = CriarElementoImovel(imoveis);

            return new Xml(cabecalhos, eImoveis);
        }
        public static IPortalXMLElementos ObterPortalXml(Portal portal, IEnumerable<PortalCaracteristica> portalCaracteristicas, string urlFotosImoveis)
        {
            switch (portal)
            {
                case Portal.Zap:
                default:
                    return new Zap(portal, portalCaracteristicas, urlFotosImoveis);
                //default:
                    //throw new NotImplementedException();
            }
        }

        private IEnumerable<ElementoImovel> CriarElementoImovel(IEnumerable<DadosImovel> imoveis)
        {
            foreach (DadosImovel imovel in imoveis)
            {
                yield return CriarElementoImovel(imovel);
            }
        }

        public static bool UsaZonaDeValor(string? estado)
        {
            if (string.IsNullOrEmpty(estado))
                return false;

            string[] estados = new [] { "SP", "RS", "RJ" };
            return estados.Any(a => a.Equals(estado));
        }

        public static string FormatarDecimal(decimal? valor)
        {
            if (!valor.HasValue)
                return string.Empty;

            return string.Format("{0,00}", valor.Value);
        }

        public static string RemoverCaracteresInvalidosUnicode(string? input, string replacement)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Regex.Replace(input, "\\p{C}+", replacement);
        }
    }
}