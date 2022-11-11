using Lopes.Anuncio.Domain.Enums;
using MediatR;

namespace Lopes.Anuncio.Domain.Commands.Requests
{
    public class CotaResumoRequest : IRequest<bool>
    {
        public CotaResumoRequest(Portal portal)
        {
            Portais = new[] { portal };
        }

        public CotaResumoRequest(Portal[] portal)
        {
            Portais = portal;
        }

        public CotaResumoRequest(int[]? idProdutos = null, int[]? idCotas = null, Portal[]? portal = null, int[]? idFranquias = null)
        {
            IdCotas = idCotas;
            Portais = portal;
            IdFranquias = idFranquias;
        }

        public int[]? IdCotas { get; set; }
        public Portal[]? Portais { get; set; }
        public int[]? IdFranquias { get; set; }
    }
}
