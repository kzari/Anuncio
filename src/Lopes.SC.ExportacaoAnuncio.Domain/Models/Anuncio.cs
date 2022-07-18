using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class Anuncio
    {
        public int IdAnuncio { get; set; }
        public int IdImovel { get; set; }
        public Portal Portal { get; set; }
        public int IdCota { get; set; }
        public int IdEmpresa { get; set; }
        public string? CodigoClientePortal { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
