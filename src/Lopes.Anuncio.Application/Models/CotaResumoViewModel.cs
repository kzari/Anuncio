using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.Models
{
    public class CotaResumoViewModel
    {
        public CotaResumoViewModel()
        {

        }

        public CotaResumoViewModel(CotaResumo cota)
        {
            Portal = cota.Portal;
            NomePortal = cota.NomePortal;
            NomeFranquia= cota.NomeFranquia;
            IdFranquia = cota.IdFranquia;
            IdCota = cota.IdCota;
            TotalProdutos = cota.TotalProdutos;
        }
        public Portal Portal { get; set; }
        public string NomePortal { get; set; }
        public int IdFranquia { get; set; }
        public string NomeFranquia { get; set; }
        public int IdCota { get; set; }
        public int TotalProdutos { get; set; }


        public string[] Portais { get; set; }
        public string[] Franquias { get; set; }

    }
}
