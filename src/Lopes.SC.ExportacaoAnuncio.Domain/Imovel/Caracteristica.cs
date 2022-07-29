namespace Lopes.SC.ExportacaoAnuncio.Domain.Imovel
{
    public class Caracteristica
    {
        public int Id { get; set; }
        public int IdImovel { get; set; }
        public string Nome { get; set; }
        public bool Unidade { get; set; }
        public bool Empreendimento { get; set; }
    }
}
