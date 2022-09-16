using Lopes.Anuncio.Domain.XML;

namespace Lopes.Anuncio.Domain.Models.XML
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
