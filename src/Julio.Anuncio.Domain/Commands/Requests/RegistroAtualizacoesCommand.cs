using Julio.Anuncio.Domain.Enums;
using Julio.Anuncio.Domain.Commands.Responses;
using MediatR;
using Julio.Anuncio.Domain.Entidades;
using Julio.Domain.Commons;

namespace Julio.Anuncio.Domain.Commands.Requests
{
    public class RegistroAtualizacoesCommand : IRequest<bool>
    {
        public RegistroAtualizacoesCommand(IEnumerable<AnuncioAtualizacao> entidades, IProgresso? progresso)
        {
            Entidades = entidades;
            Progresso = progresso;
        }

        public IProgresso? Progresso { get; set; }
        public IEnumerable<AnuncioAtualizacao> Entidades { get; set; }
    }

    public class RegistroAtualizacaoCommand : IRequest<AtualizarStatusAnuncioResponse>
    {
        public RegistroAtualizacaoCommand(Portal idPortal, int idProduto, int idEmpresa, AtualizacaoAcao acao, DateTime? data = null, Guid? id = null)
        {
            IdPortal = idPortal;
            IdProduto = idProduto;
            IdEmpresa = idEmpresa;
            Acao = acao;
            Data = data ?? DateTime.Now;
            Id = id ?? Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Portal IdPortal { get; set; }
        public int IdProduto { get; set; }
        public int IdEmpresa { get; set; }
        public AtualizacaoAcao Acao { get; set; }
        public DateTime Data { get; set; }
    }
}