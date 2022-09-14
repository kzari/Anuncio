using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Commands.Responses;
using MediatR;
using Lopes.Anuncio.Domain.Entidades;

namespace Lopes.Anuncio.Domain.Commands.Requests
{
    public class RegistroAtualizacoesCommand : IRequest<bool>
    {
        public RegistroAtualizacoesCommand(IEnumerable<AnuncioAtualizacao> entidades)
        {
            Entidades = entidades;
        }

        public IEnumerable<AnuncioAtualizacao> Entidades { get; set; }
    }

    public class RegistroAtualizacaoCommand : IRequest<AtualizarStatusAnuncioResponse>
    {
        public RegistroAtualizacaoCommand(Portal idPortal, int idImovel, int idEmpresa, AtualizacaoAcao acao, DateTime? data = null, Guid? id = null)
        {
            IdPortal = idPortal;
            IdImovel = idImovel;
            IdEmpresa = idEmpresa;
            Acao = acao;
            Data = data ?? DateTime.Now;
            Id = id ?? Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Portal IdPortal { get; set; }
        public int IdImovel { get; set; }
        public int IdEmpresa { get; set; }
        public AtualizacaoAcao Acao { get; set; }
        public DateTime Data { get; set; }
    }
}