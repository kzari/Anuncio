namespace Lopes.Anuncio.Domain.XML
{
    public class ElementoImovel : Elemento
    {
        public ElementoImovel(int idImovel, string nome, string? valor = null, IEnumerable<Atributo> atributos = null, IEnumerable<Elemento> elementosFilhos = null) : 
            base(nome, valor, atributos, elementosFilhos)
        {
            IdImovel = idImovel;
        }

        public int IdImovel { get; set; }
    }
    public class Elemento
    {
        public Elemento(string nome, string? valor = null, IEnumerable<Atributo> atributos = null, IEnumerable<Elemento> elementosFilhos = null)
        {
            Nome = nome;
            if (valor != null)
                Valor = valor;
                
            Atributos = atributos?.ToList() ?? new List<Atributo>();
            Filhos = elementosFilhos?.ToList() ?? new List<Elemento>();
        }

        public string Nome { get; set; }
        public string? Valor { get; set; }
        public List<Atributo> Atributos { get; set; }
        public List<Elemento> Filhos { get; set; }

        public Elemento? AdicionarElemento(string nome, 
                                           string? valor = null, 
                                           IEnumerable<Atributo> atributos = null, 
                                           IEnumerable<Elemento> elementosFilhos = null,
                                           bool naoAdicionarSeNuloOuVazio = false)
        {
            if (naoAdicionarSeNuloOuVazio && string.IsNullOrEmpty(valor))
                return null;

            var elemento = new Elemento(nome, valor, atributos, elementosFilhos);
            Filhos.Add(elemento);
            return elemento;
        }
    }
}