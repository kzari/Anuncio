using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Domain.Commons;
using MediatR;

namespace Lopes.Anuncio.Domain.Commands.Requests
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
