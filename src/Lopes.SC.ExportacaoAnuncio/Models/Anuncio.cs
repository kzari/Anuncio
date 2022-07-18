using Lopes.SC.ExportacaoAnuncio.Domain.Enums;

namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class Anuncio
    {
        public int IdAnuncio { get; set; }
        public int IdImovel { get; set; }

        public int IdEmpresa { get; set; }
        public string Empresa { get; set; }

        public int IdCota { get; set; }

        public Portal Portal { get; set; }
        public int IdSubveiculo { get; set; }
        public string Subveiculo { get; set; }
        public int CdAnuncioClassificacao { get; set; }

        public DateTime ImovelUltimaAlteracao { get; set; }

        public StatusImovel StatusImovel { get; set; }
        public string NomeStatusImovel { get; set; }

        public int IdAnuncioStatus { get; set; }
        public string AnuncioStatus { get; set; }

        public int IdCotaStatus { get; set; }
        public string CotaStatus { get; set; }

        public bool AnuncioLiberado { get; set; }
        public bool PodeAnunciarOutraLoja { get; set; }

        public DateTime? DataAtualizacao { get; set; }    
        public DateTime? DataRemocao { get; set; }
        public string? CodigoClientePortal { get; set; }

        /// <summary>
        /// o Imóvel está mais atualizado que o anúncio
        /// </summary>
        public bool ImovelAtualizado => !DataAtualizacao.HasValue ||
                                         DataAtualizacao < ImovelUltimaAlteracao;
    }  
}
