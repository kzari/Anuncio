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
        private readonly IAnuncioStatusRepositorio _repositorio;

        public RegistroAtualizacaoCommandHandler(IAnuncioStatusRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

          
        public async Task<bool> Handle(RegistroAtualizacoesCommand command, CancellationToken cancellationToken)
        {
            //TODO: validar

            await _repositorio.CriarAsync(command.Entidades, command.Progresso);

            return await Task.FromResult(true);
        }

        public async Task<AtualizarStatusAnuncioResponse> Handle(RegistroAtualizacaoCommand request, CancellationToken cancellationToken)
        {
            var entidade = new AnuncioAtualizacao(request.IdPortal, request.IdProduto, request.IdEmpresa, request.Acao, request.Id, request.Data);
            //TODO: validar

            _repositorio.Criar(entidade);

            return await Task.FromResult(new AtualizarStatusAnuncioResponse(request));
        }
    }
}
