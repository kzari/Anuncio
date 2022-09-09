namespace Lopes.Anuncio.Domain.XML
{
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