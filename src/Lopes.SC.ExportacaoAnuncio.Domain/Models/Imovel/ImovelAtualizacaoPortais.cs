using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class AnuncioAtualizacao
    {
        public AnuncioAtualizacao(Portal idPortal, int idImovel, int idEmpresa, AtualizacaoAcao acao, DateTime? data = null)
        {
            IdPortal = idPortal;
            IdImovel = idImovel;
            IdEmpresa = idEmpresa;
            Acao = acao;
            Data = data ?? DateTime.Now;
        }

        private AnuncioAtualizacao()
        {
            //EF ctor
        }

        public int Id { get; set; }
        public Portal IdPortal { get; set; }
        public int IdImovel { get; set; }
        public int IdEmpresa { get; set; }
        public AtualizacaoAcao Acao { get; set; }
        public DateTime Data { get; set; }
    }
}
