using Julio.Anuncio.Domain.Enums;
using MediatR;

namespace Julio.Anuncio.Domain.Commands.Requests
{
    public class CotaResumoRequest : IRequest<bool>
    {
        public CotaResumoRequest(int portal)
        {
            Portais = new[] { portal };
        }

        public CotaResumoRequest(int[] portal)
        {
            Portais = portal;
        }

        public CotaResumoRequest(int[]? idProdutos = null, int[]? idCotas = null, int[]? portal = null, int[]? idFranquias = null)
        {
            IdCotas = idCotas;
            Portais = portal;
            IdFranquias = idFranquias;
        }

        public int[]? IdCotas { get; set; }
        public int[]? Portais { get; set; }
        public int[]? IdFranquias { get; set; }
    }
}
