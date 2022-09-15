using Lopes.Domain.Commons;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Commands.Requests;
using MediatR;
using Lopes.Anuncio.Domain.ObjetosValor;

namespace Lopes.Anuncio.Application.Services
{
    public class AtualizacaoAppService : IAtualizacaoAppService
    {
        private readonly IMediator _mediator;
        private readonly IAnuncioRepository _repositorio;

        public AtualizacaoAppService(IMediator mediator, IAnuncioRepository repositorio)
        {
            _mediator = mediator;
            _repositorio = repositorio;
        }

        public void AtualizarAnuncios(AnuncioCotaRequest request, ILogger? logger)
        {
            IEnumerable<AnuncioCota> anuncios = _repositorio.Obter(request).OrderBy(_ => _.IdImovel).ToList();

            AnunciosAtualizacaoCommand anunciosCommand = new(anuncios, logger);

            bool result = _mediator.Send(anunciosCommand).Result;
        }
    }
}