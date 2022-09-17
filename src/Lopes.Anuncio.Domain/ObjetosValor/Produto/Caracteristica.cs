namespace Lopes.Anuncio.Domain.Models.DadosProduto
{
    public class Caracteristica
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public bool Unidade { get; set; }
        public bool Empreendimento { get; set; }
    }
}
