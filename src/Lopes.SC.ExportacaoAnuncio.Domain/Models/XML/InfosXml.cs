using Lopes.SC.ExportacaoAnuncio.Domain.XML;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models.XML
{
    public class Xml
    {
        public Xml(Elemento cabecalhos, IEnumerable<ElementoImovel> eImoveis, string caminhoTagPaiImoveis)
        {
            Cabecalhos = cabecalhos;
            Imoveis = eImoveis;
            CaminhoTagPaiImoveis = caminhoTagPaiImoveis;
        }

        public Elemento Cabecalhos { get; set; }
        public IEnumerable<ElementoImovel> Imoveis { get; set; }
        public string CaminhoTagPaiImoveis { get; set; }
    }
}
