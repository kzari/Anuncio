using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Domain.Models
{
    public class PortalCaracteristica
    {
        public Portal Portal { get; set; }
        public int IdCaracteristica { get; set; }
        public string Tag { get; set; }
    }
}
