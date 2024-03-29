﻿using Julio.Anuncio.Domain.Enums;

namespace Julio.Anuncio.Domain.ObjetosValor
{
    public class AnuncioStatus
    {
        public AnuncioStatus(AnuncioCota anuncioCota, StatusAnuncioPortal? statusAnuncioPortal = null)
        {
            AnuncioCota = anuncioCota;
            Status = statusAnuncioPortal;
        }

        public AnuncioCota AnuncioCota { get; }
        public StatusAnuncioPortal? Status { get; set; }
    }

    public class AnuncioCota 
    {
        public int IdAnuncio { get; set; }
        public int IdProduto { get; set; }

        public int IdFranquia { get; set; }
        public string NomeFranquia { get; set; }

        public int IdCota { get; set; }
        public Portal Portal { get; set; }

        public DateTime? ProdutoUltimaAlteracao { get; set; }
        public ProdutoStatus IdStatusProduto { get; set; }

        public int IdStatusAnuncio { get; set; }
        public int IdStatusCota { get; set; }

        public bool AnuncioLiberado { get; set; }
        public bool PodeAnunciarOutraFranquia { get; set; }

        public string? CodigoClientePortal { get; set; }

        public DateTime? DataAtualizacaoAnuncioPortal { get; set; }
        public AtualizacaoAcao? AcaoAtualizacaoAnuncioPortal { get; set; }

        /// <summary>
        /// True se anuncio precisa ser atualizado
        /// </summary>
        public bool AnuncioDesatualizado => !DataAtualizacaoAnuncioPortal.HasValue || DataAtualizacaoAnuncioPortal.Value < ProdutoUltimaAlteracao;

        public bool Ativo => IdStatusAnuncio == 1;
        public bool CotaAtiva => IdStatusCota == 1;
        public bool ProdutoAtivo => IdStatusProduto == ProdutoStatus.Ativo;
    }
}
