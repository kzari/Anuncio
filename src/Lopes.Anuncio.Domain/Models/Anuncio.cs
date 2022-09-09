using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Anuncio.Domain.Models
{
    public class AnuncioImovel
    {
        public int IdAnuncio { get; set; }
        public int IdImovel { get; set; }

        public int IdEmpresa { get; set; }
        public string NomeEmpresa { get; set; }

        public int IdCota { get; set; }
        public Portal Portal { get; set; }

        public DateTime? ImovelUltimaAlteracao { get; set; }
        public StatusImovel IdStatusImovel { get; set; }

        public int IdStatusAnuncio { get; set; }
        public int IdStatusCota { get; set; }

        public bool AnuncioLiberado { get; set; }
        public bool PodeAnunciarOutraEmpresa { get; set; }

        public string? CodigoClientePortal { get; set; }

        public DateTime? DataAtualizacaoAnuncioPortal { get; set; }
        public AtualizacaoAcao? AcaoAtualizacaoAnuncioPortal { get; set; }

        /// <summary>
        /// True se anuncio precisa ser atualizado
        /// </summary>
        public bool AnuncioDesatualizado => !DataAtualizacaoAnuncioPortal.HasValue || DataAtualizacaoAnuncioPortal.Value < ImovelUltimaAlteracao;

        public bool Ativo => IdStatusAnuncio == 1;
        public bool CotaAtiva => IdStatusCota == 1;
        public bool ImovelAtivo => IdStatusImovel == StatusImovel.Ativo;
    }
}
