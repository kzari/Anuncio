namespace Lopes.Anuncio.Application.Models
{
    public class AnunciosResumoViewModel
    {
        public AnunciosResumoViewModel()
        {

        }

        public AnunciosResumoViewModel(IEnumerable<CotaResumoViewModel> cotaResumo)
        {
            Cotas = cotaResumo.ToList();
            TotalAnuncios = Cotas.Sum(_ => _.TotalProdutos);
            TotalCotas = Cotas.Count();
            TotalFranquias = Cotas.Select(_ => _.IdFranquia).Distinct().Count();
            TotalPortais = Cotas.Select(_ => _.Portal).Distinct().Count();
        }

        public List<CotaResumoViewModel> Cotas { get; set; }
        public int TotalAnuncios { get; set; }
        public int TotalCotas { get; set; }
        public int TotalFranquias { get; set; }
        public int TotalPortais { get; set; }
        public int TotalImoveis { get; set; }
    }
}
