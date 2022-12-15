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
            CotaAtiva = cota.CotaAtiva;
            TotalProdutos = cota.TotalProdutos;
        }
        public int Portal { get; set; }
        public string NomePortal { get; set; }
        public int IdFranquia { get; set; }
        public string NomeFranquia { get; set; }
        public int IdCota { get; set; }
        public bool CotaAtiva { get; set; }
        public int TotalProdutos { get; set; }
    }
}
