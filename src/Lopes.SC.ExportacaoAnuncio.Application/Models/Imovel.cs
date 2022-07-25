using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;

namespace Lopes.SC.ExportacaoAnuncio.Application.Models
{
    public class Imovel
    {
        public Imovel(DadosPrincipais dados)
        {
            IdImovel = dados.IdImovel;
            Titulo = dados.Titulo ?? string.Empty;
            TextoSite = dados.TextoSite ?? string.Empty;

            AreaTotal = dados.AreaTotal;
            AreaUtil = dados.AreaPrivativa;
            AreaPrivativa = dados.AreaPrivativa;

            QtdeVagas = dados.QtdeVagas ?? 0;
            QtdeVagasDemarcadas = dados.QtdeVagasDemarcadas ?? 0;
            QtdeVagasNaoDemarcadas = dados.QtdeVagasNaoDemarcadas ?? 0;
            QtdeQuartos = dados.QtdeQuartos ?? 0;
            QtdeSuites = dados.QtdeSuites ?? 0;
            QtdeSalas = dados.QtdeSalas ?? 0;
            QtdeBanheirosSociais = dados.QtdeBanheirosSociais ?? 0;
            ValorVenda = dados.ValorVenda ?? 0;
            ValorLocacao = dados.ValorLocacao ?? 0;
            ValorIPTU = dados.ValorIPTU ?? 0;
            ValorCondominio = dados.ValorCondominio ?? 0;
            Tipo = dados.Tipo ?? string.Empty;
            Subtipo = dados.Subtipo ?? string.Empty;
            Ranking = dados.Ranking;
            DataInclusao = dados.DataInclusao;
            Logradouro = dados.Logradouro ?? string.Empty;
            Numero = dados.Numero ?? string.Empty;
            Complemento = dados.Complemento ?? string.Empty;
            Cidade = dados.Cidade ?? string.Empty;
            Bairro = dados.Bairro ?? string.Empty;
            Estado = dados.Estado ?? string.Empty;
            CEP = dados.CEP ?? string.Empty;

            Latitude = dados.Latitude;
            Longitude = dados.Longitude;

            SA = dados.SA;
            AnoConstrucao = dados.AnoConstrucao;
        }

        public int IdImovel { get; set; }
        public string IdImovelNoPortal => $"REO{IdImovel}";

        public string Titulo { get; set; }
        public string TextoSite { get; set; }

        public decimal? AreaTotal { get; set; }
        public decimal? AreaPrivativa { get; set; }
        public decimal? AreaUtil { get; set; }

        public int QtdeVagas { get; set; }
        public int QtdeVagasDemarcadas { get; set; }
        public int QtdeVagasNaoDemarcadas { get; set; }
        public int QtdeQuartos { get; set; }
        public int QtdeSuites { get; set; }
        public int QtdeSalas { get; set; }
        public int QtdeBanheirosSociais { get; set; }

        public decimal? ValorVenda { get; set; }
        public decimal? ValorLocacao { get; set; }
        public decimal? ValorIPTU { get; set; }
        public decimal? ValorCondominio { get; set; }

        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public int? Ranking { get; set; }
        public DateTime DataInclusao { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public string? SA { get; set; }

        //public string? GeographicalZone_Id { get; set; }
        //public string? ValueZone { get; set; }
        //public string? LatitudeRealty { get; set; }
        //public string? LongitudeRealty { get; set; }
        public int? AnoConstrucao { get; set; }
        public string? CodigoClientePortal { get; set; }

        public IEnumerable<Caracteristica> Caracteristicas { get; set; }
        public IEnumerable<ImovelImagem> Imagens { get; set; }
        public IEnumerable<string> UrlVideos { get; set; }
        public IEnumerable<string> UrlTourVirtuais { get; set; }
    }
}
