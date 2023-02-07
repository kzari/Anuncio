using Julio.Anuncio.Domain.XML;

namespace Julio.Anuncio.Domain.Models.XML
{
    public class Xml
    {
        public Xml(Elemento cabecalhos, IEnumerable<ElementoProduto> eProdutos)
        {
            Cabecalhos = cabecalhos;
            Produtos = eProdutos;
        }

        public Elemento Cabecalhos { get; set; }
        public IEnumerable<ElementoProduto> Produtos { get; set; }
    }
}
