using Lopes.Anuncio.Domain.Enums;
using MediatR;

namespace Lopes.Anuncio.Domain.Commands.Requests
{
    public class AnuncioCotaRequest : IRequest<bool>
    {
        public AnuncioCotaRequest(Portal portal)
        {
            Portais = new[] { portal };
        }

        public AnuncioCotaRequest(Portal[] portal)
        {
            Portais = portal;
        }

        public AnuncioCotaRequest(int[]? idProdutos = null, int[]? idCotas = null, Portal[]? portal = null)
        {
            IdProdutos = idProdutos;
            IdCotas = idCotas;
            Portais = portal;
        }

        public int[]? IdProdutos { get; set; }
        public int[]? IdCotas { get; set; }
        public Portal[]? Portais { get; set; }
    }
}
