namespace Julio.Anuncio.Domain.Models
{
    public class Foto
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
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

        public string ObterCaminhoFotoProduto(string urlFotos) => $"{urlFotos}REO{IdProduto}/{NomeArquivo}";
    }
}
