using Lopes.Anuncio.Domain.Enums;

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

            var preferences = new List<int> { (int)Portal.Zap };
            Portais = Cotas.GroupBy(_ => new { _.Portal, _.NomePortal })
                           .Select(_ => new ItemCotas((int)_.Key.Portal, _.Key.NomePortal, _.Count()))
                           .OrderBy(d => {
                               var index = preferences.IndexOf(d.Id);
                               return index == -1 ? int.MaxValue : index;
                           })
                           .ToArray();
            Franquias = Cotas.GroupBy(_ => new { _.IdFranquia, _.NomeFranquia })
                             .Select(_ => new ItemCotas(_.Key.IdFranquia, _.Key.NomeFranquia, _.Count()))
                             .OrderBy(_ => _.Nome)
                             .ToArray();

            TotalFranquias = Franquias.Count();
            TotalPortais = Portais.Count();
        }

        public List<CotaResumoViewModel> Cotas { get; set; }
        public int TotalAnuncios { get; set; }
        public int TotalCotas { get; set; }
        public int TotalFranquias { get; set; }
        public int TotalPortais { get; set; }
        public int TotalImoveis { get; set; }

        public ItemCotas[] Portais { get; set; }
        public ItemCotas[] Franquias { get; set; }
    }

    public struct ItemCotas
    {
        public ItemCotas(int id, string nome, int qtdeCotas)
        {
            Id = id;
            Nome = nome;
            QtdeCotas = qtdeCotas;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int QtdeCotas { get; set; }
    }
}
