using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class ImovelCaracteristica
    {
    }

    public class Anuncio
    {
        public int IdImovel { get; set; }
        public Portal Portal { get; set; }
        public int IdEmpresa { get; set; }
        public string? CodigoClientePortal { get; set; }
    }
}
