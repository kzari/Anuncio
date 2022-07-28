using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class PortalCaracteristica
    {
        public Portal Portal { get; set; }
        public int IdCaracteristica { get; set; }
        public string Tag { get; set; }
    }
}
