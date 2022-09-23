namespace Lopes.Anuncio.Domain.XML
{
    public class ElementoProduto : Elemento
    {
        public ElementoProduto(int idProduto, string nome, string? valor = null, IEnumerable<Atributo> atributos = null, IEnumerable<Elemento> elementosFilhos = null) :
            base(nome, valor, atributos, elementosFilhos)
        {
            IdProduto = idProduto;
        }

        public int IdProduto { get; }
    }
}