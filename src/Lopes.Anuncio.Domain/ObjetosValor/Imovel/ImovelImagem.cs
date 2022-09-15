namespace Lopes.Anuncio.Domain.Models
{
    public class Fotos
    {
        public int Id { get; set; }
        public int IdImovel { get; set; }
        public string NomeArquivo { get; set; }
        public int Ordem { get; set; }


        private string descricao;
        public string Descricao
        {
            get { return descricao; }
            set
            {
                if(!string.IsNullOrEmpty(value) && string.Equals(value, "sem categoria", StringComparison.OrdinalIgnoreCase))
                    descricao = string.Empty;
                descricao = value;
            }
        }

        public string ObterCaminhoFotoImovel(string urlFotos) => $"{urlFotos}REO{IdImovel}/{NomeArquivo}";
    }
}
