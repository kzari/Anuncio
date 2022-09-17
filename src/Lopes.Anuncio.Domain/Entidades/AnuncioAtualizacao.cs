using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Domain.Entidades
{
    public class AnuncioAtualizacao
    {
        public AnuncioAtualizacao(Portal idPortal, int idProduto, int idEmpresa, AtualizacaoAcao acao, Guid? id = null, DateTime? data = null)
        {
            IdPortal = idPortal;
            IdProduto = idProduto;
            IdFranquia = idEmpresa;
            Acao = acao;
            Data = data ?? DateTime.Now;
            Id = id ?? Guid.NewGuid();
        }

        private AnuncioAtualizacao()
        {
            //EF ctor
        }

        public Guid Id { get; set; }
        public Portal IdPortal { get; set; }
        public int IdProduto { get; set; }
        public int IdFranquia { get; set; }
        public AtualizacaoAcao Acao { get; set; }
        public DateTime Data { get; set; }
    }
}
