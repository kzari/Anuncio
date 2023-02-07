namespace Julio.Anuncio.Domain.XML
{
    public class Elemento
    {
        public Elemento(string nome, 
                        string? valor = null, 
                        IEnumerable<Atributo> atributos = null, 
                        IEnumerable<Elemento> elementosFilhos = null)
        {
            Nome = nome;
            Valor = valor;
            Atributos = atributos?.ToList();
            Filhos = elementosFilhos?.ToList();
        }

        public string Nome { get; }
        public string? Valor { get; }
        public List<Atributo>? Atributos { get; }
        public List<Elemento>? Filhos { get; private set; }

        /// <summary>
        /// Adiciona um elemento filho a este elemento
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="valor"></param>
        /// <param name="naoAdicionarSeNuloOuVazio">Não adiciona o elemento filho caso o valor seja vazio ou nulo</param>
        /// <returns></returns>
        public Elemento? AdicionarFilho(string nome, string? valor = null, bool naoAdicionarSeNuloOuVazio = false)
        {
            return AdicionarFilho(new Elemento(nome, valor), naoAdicionarSeNuloOuVazio);
        }

        /// <summary>
        /// Adiciona um elemento filho a este elemento
        /// </summary>
        /// <param name="filho"></param>
        /// <param name="naoAdicionarSeNuloOuVazio">Não adiciona o elemento filho caso o valor seja vazio ou nulo</param>
        /// <returns></returns>
        public Elemento? AdicionarFilho(Elemento filho, bool naoAdicionarSeNuloOuVazio = false)
        {
            if (naoAdicionarSeNuloOuVazio && string.IsNullOrEmpty(filho.Valor))
                return null;

            (Filhos ??= new List<Elemento>()).Add(filho);
            return filho;
        }
    }
}