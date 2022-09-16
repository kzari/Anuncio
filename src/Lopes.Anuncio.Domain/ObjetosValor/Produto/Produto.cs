using Lopes.Anuncio.Domain.Models;

namespace Lopes.Anuncio.Domain.Models.DadosProduto
{
    public class Produto
    {
        public Produto(DadosPrincipais principais)
        {
            Dados = principais;
        }

        public string? CodigoClientePortal { get; set; }
        public DadosPrincipais Dados { get; set; }
        public IEnumerable<Caracteristica> Caracteristicas { get; set; }
        public IEnumerable<string> UrlTourVirtuais { get; set; }
        public IEnumerable<string> UrlVideos { get; set; }
        public IEnumerable<Foto> Imagens { get; set; }
    }
}
