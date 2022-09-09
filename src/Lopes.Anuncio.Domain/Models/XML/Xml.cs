using Lopes.Anuncio.Domain.XML;

namespace Lopes.Anuncio.Domain.Models.XML
{
    public class Xml
    {
        public Xml(Elemento cabecalhos, IEnumerable<ElementoImovel> eImoveis)
        {
            Cabecalhos = cabecalhos;
            Imoveis = eImoveis;
        }

        public Elemento Cabecalhos { get; set; }
        public IEnumerable<ElementoImovel> Imoveis { get; set; }
    }
}
