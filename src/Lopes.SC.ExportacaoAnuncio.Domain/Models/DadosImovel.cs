﻿namespace Lopes.SC.ExportacaoAnuncio.Domain.Models
{
    public class DadosImovel
    {
        public int IdImovel { get; set; }
        public string? Titulo { get; set; }
        public string? TextoSite { get; set; }
        public decimal? AreaTotal { get; set; }
        public decimal? AreaPrivativa { get; set; }
        public int? QtdeVagas { get; set; }
        public int? QtdeVagasDemarcadas { get; set; }
        public int? QtdeVagasNaoDemarcadas { get; set; }
        public int? QtdeQuartos { get; set; }
        public int? QtdeSuites { get; set; }
        public int? QtdeSalas { get; set; }
        public int? QtdeBanheirosSociais { get; set; }
        public int? ValorVenda { get; set; }
        public int? ValorLocacao { get; set; }
        public int? ValorIPTU { get; set; }
        public int? ValorCondominio { get; set; }
        public string? Tipo { get; set; }
        public string? Subtipo { get; set; }
        public int? Ranking { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cidade { get; set; }
        public string? Bairro { get; set; }
        public string? Estado { get; set; }
        public string? CEP { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? SA { get; set; }
        public int? AnoConstrucao { get; set; }
    }
}