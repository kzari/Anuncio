namespace Lopes.SC.AnuncioXML.Domain.Models
{
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

        public Elemento AdicionarElemento(string nome, 
                                          string valor = null, 
                                          IEnumerable<Atributo> atributos = null, 
                                          IEnumerable<Elemento> elementosFilhos = null, 
                                          bool naoAdicionarSeValorNulo = false)
        {
            var elemento = new Elemento(nome, valor, atributos, elementosFilhos);
            Filhos.Add(elemento);
            return elemento;
        }
    }

    public struct Atributo
    {
        public Atributo(string nome, string valor)
        {
            Nome = nome;
            Valor = valor;
        }

        public string Nome { get; }
        public string Valor { get; }
    }
}