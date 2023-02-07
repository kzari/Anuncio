using Julio.Domain.Commons;
using Julio.Anuncio.Application.Interfaces;
using Julio.Anuncio.Application.DadosService;
using Julio.Anuncio.Domain.Commands.Requests;
using MediatR;
using Julio.Anuncio.Domain.ObjetosValor;
using Julio.Acesso.Commons.Extensions;

namespace Julio.Anuncio.Application.Services
{
    public class AtualizacaoAppService : IAtualizacaoAppService
    {
        private readonly IMediator _mediator;
        private readonly IAnuncioDadosService _dadosService;

        public AtualizacaoAppService(IMediator mediator, IAnuncioDadosService repositorio)
        {
            _mediator = mediator;
            _dadosService = repositorio;
        }

        public void AtualizarAnuncios(AnuncioCotaRequest request, ILogger? logger)
        {
            if (request == null || (request.Portais.Nenhum() && request.IdCotas.Nenhum() && request.IdProdutos.Nenhum()))
                throw new Exception("Nenhum filtro foi passado para selecionar para os anúncios. Filtre por Portais, Cotas e ou Produtos.");
                
            IEnumerable<AnuncioCota> anuncios = _dadosService.Obter(request).OrderBy(_ => _.IdProduto).ToList();

            AnunciosAtualizacaoCommand anunciosCommand = new(anuncios, logger);

            bool result = _mediator.Send(anunciosCommand).Result;
        }
    }
}