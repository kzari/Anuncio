using Julio.Anuncio.Domain.Commands.Responses;
using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Domain.Commons;
using MediatR;

namespace Julio.Anuncio.Domain.Commands.Requests
{
    public class AnunciosAtualizacaoCommand : IRequest<bool>
    {
        public AnunciosAtualizacaoCommand(IEnumerable<AnuncioCota> anuncios, ILogger? logger = null)
        {
            Anuncios = anuncios;
            Logger = logger;
        }

        public IEnumerable<AnuncioCota> Anuncios { get; set; }
        public ILogger? Logger { get; set; }
    }
}
