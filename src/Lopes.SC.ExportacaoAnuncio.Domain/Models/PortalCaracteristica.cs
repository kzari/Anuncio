using Lopes.SC.Anuncio.Domain.Enums;

namespace Lopes.SC.Anuncio.Domain.Models
{
    public class PortalCaracteristica
    {
        public Portal Portal { get; set; }
        public int IdCaracteristica { get; set; }
        public string Tag { get; set; }
    }
}
