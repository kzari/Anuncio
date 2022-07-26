using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Models.XML;
using Lopes.SC.ExportacaoAnuncio.Domain.XML;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Services
{
    public abstract class PortalXmlElementosBase
    {
        protected abstract string CaminhoTagPaiImoveis { get; }
        protected abstract Elemento CriarElementoCabecalho();
        protected abstract ElementoImovel CriarElementoImovel(DadosImovel dados);

        public Xml ObterXml(DadosImovel dados) => ObterXml(new List<DadosImovel> { dados });
        public Xml ObterXml(IEnumerable<DadosImovel> imoveis)
        {
            Elemento cabecalhos = CriarElementoCabecalho();

            IEnumerable<ElementoImovel> eImoveis = CriarElementoImovel(imoveis);

            return new Xml(cabecalhos, eImoveis, CaminhoTagPaiImoveis);
        }
        public static IPortalXMLElementos ObterPortalXml(Portal portal)
        {
            switch (portal)
            {
                case Portal.Zap:
                    return new Zap();
                default:
                    return new Zap();
            }
        }

        private IEnumerable<ElementoImovel> CriarElementoImovel(IEnumerable<DadosImovel> imoveis)
        {
            foreach (DadosImovel imovel in imoveis)
            {
                yield return CriarElementoImovel(imovel);
            }
        }
    }
}