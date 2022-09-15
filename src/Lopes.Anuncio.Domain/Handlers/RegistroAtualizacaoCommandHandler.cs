using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Commands.Responses;
using MediatR;
using Lopes.Anuncio.Domain.Entidades;

namespace Lopes.Anuncio.Domain.Handlers
{
    public class RegistroAtualizacaoCommandHandler : IRequestHandler<RegistroAtualizacoesCommand, bool>,
                                              IRequestHandler<RegistroAtualizacaoCommand, AtualizarStatusAnuncioResponse>
    {
        private readonly IAnuncioStatusRepositorioGravacao _repository;

        public RegistroAtualizacaoCommandHandler(IAnuncioStatusRepositorioGravacao repository)
        {
            _repository = repository;
        }


        public Task<bool> Handle(RegistroAtualizacoesCommand command, CancellationToken cancellationToken)
        {
            //TODO: validar

            _repository.Criar(command.Entidades);

            return Task.FromResult(true);
        }

        public Task<AtualizarStatusAnuncioResponse> Handle(RegistroAtualizacaoCommand request, CancellationToken cancellationToken)
        {
            var entidade = new AnuncioAtualizacao(request.IdPortal, request.IdImovel, request.IdEmpresa, request.Acao, request.Id, request.Data);
            //TODO: validar

            _repository.Criar(entidade);

            return Task.FromResult(new AtualizarStatusAnuncioResponse(request));
        }
    }
}
