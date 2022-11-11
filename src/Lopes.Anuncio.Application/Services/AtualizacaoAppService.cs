using Lopes.Domain.Commons;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Domain.Commands.Requests;
using MediatR;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Acesso.Commons.Extensions;

namespace Lopes.Anuncio.Application.Services
{
    public class AtualizacaoAppService : IAtualizacaoAppService
    {
        private readonly IMediator _mediator;
        private readonly IAnuncioDadosService _repositorio;

        public AtualizacaoAppService(IMediator mediator, IAnuncioDadosService repositorio)
        {
            _mediator = mediator;
            _repositorio = repositorio;
        }

        public void AtualizarAnuncios(AnuncioCotaRequest request, ILogger? logger)
        {
            if (request == null || (request.Portais.Nenhum() && request.IdCotas.Nenhum() && request.IdProdutos.Nenhum()))
                throw new Exception("Nenhum filtro foi passado para selecionar para os anúncios. Filtre por Portais, Cotas e ou Produtos.");
                
            IEnumerable<AnuncioCota> anuncios = _repositorio.Obter(request).OrderBy(_ => _.IdProduto).ToList();

            AnunciosAtualizacaoCommand anunciosCommand = new(anuncios, logger);

            bool result = _mediator.Send(anunciosCommand).Result;
        }
    }
}