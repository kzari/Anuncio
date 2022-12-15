using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Domain.ObjetosValor
{
    public class CotaResumo
    {
        public int Portal {get;set;}
        public string NomePortal { get; set; }
        public int IdFranquia { get; set; }
        public string NomeFranquia { get; set; }
        public int IdCota { get; set; }
        public int IdStatusCota { get; set; }
        public bool CotaAtiva => IdStatusCota == 1;
        public int TotalProdutos { get; set; } 
    }
}
