using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Imovel;
using Lopes.Anuncio.Domain.Models;
using Lopes.Anuncio.Domain.Services;

namespace Lopes.Anuncio.Domain.XML
{
    public class Zap : PortalXmlElementosBase, IPortalXMLElementos
    {
        public Zap(Portal portal, IEnumerable<PortalCaracteristica> portalCaracteristicas, string urlFotosImoveis) : base(portal, portalCaracteristicas, urlFotosImoveis)
        {
        }

        public string CaminhoTagPaiImoveis => "/Carga/Imoveis";
        public string NomeTagImovel => "Imovel";
        public string NomeTagCodigoImovel => "CodigoImovel";

        protected override Elemento CriarElementoCabecalho()
        {
            var eCarga = new Elemento("Carga", null, new List<Atributo>
            {
                new Atributo("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new Atributo("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
            });

            eCarga.AdicionarElemento(nome: "Imoveis");

            return eCarga;
        }

        protected override ElementoImovel CriarElementoImovel(DadosImovel dados)
        {
            ElementoImovel eImovel = new ElementoImovel(dados.Dados.IdImovel, "Imovel");

            DeterminaTipoSubtipoCategoriaDoImovel(dados.Dados, out string tipoImovel, out string subTipoImovel, out string categoria);

            eImovel.AdicionarElemento("CodigoImovel", dados.Dados.IdImovelPortais);

            eImovel.AdicionarElemento("TipoImovel", dados.Dados.Tipo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("SubTipoImovel", dados.Dados.Subtipo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("TituloImovel", dados.Dados.Titulo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Observacao", RemoverCaracteresInvalidosUnicode(dados.Dados.TextoSite, " "), naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarElemento("InscricaoMunicipal", dados.Dados.InscricaoMunicipal, naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarElemento("QtdDormitorios", DefinirQtdeComodos(dados.Dados.QtdeQuartos, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("QtdSuites", DefinirQtdeComodos(dados.Dados.QtdeSuites, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("QtdBanheiros", DefinirQtdeComodos(dados.Dados.QtdeBanheirosSociais, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("QtdVagas", DefinirQtdeComodos(dados.Dados.QtdeVagas, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarElemento("AnoConstrucao", dados.Dados.AnoConstrucao?.ToString(), naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarElemento("AreaUtil", FormatarDecimal(dados.Dados.AreaPrivativa), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("AreaTotal", FormatarDecimal(dados.Dados.AreaTotal), naoAdicionarSeNuloOuVazio: true);

            AdicionarEndereco(dados.Dados, eImovel);

            AdicionarValores(dados.Dados, eImovel);

            AdicionarVideos(dados, eImovel);

            AdicionarTourVirtuais(dados, eImovel);

            AdicionarCaracteristicas(dados, eImovel);

            AdicionarFotos(dados, eImovel);

            return eImovel;
        }

        private void AdicionarFotos(DadosImovel dados, ElementoImovel eImovel)
        {
            if (!dados.Imagens.Any())
                return;

            Elemento eFotos = eImovel.AdicionarElemento("Fotos");
            foreach (Fotos foto in dados.Imagens)
            {
                Elemento eFoto = eFotos.AdicionarElemento("Foto");
                eFoto.AdicionarElemento("NomeArquivo", foto.Descricao);
                eFoto.AdicionarElemento("Principal", foto.Ordem == 1 ? "1" : "0");
                eFoto.AdicionarElemento("URLArquivo", foto.ObterCaminhoFotoImovel(UrlFotosImoveis));
                eFoto.AdicionarElemento("Alterada", "1");
            }
        }

        private static void AdicionarValores(DadosPrincipais dados, ElementoImovel eImovel)
        {
            eImovel.AdicionarElemento("PrecoVenda", FormatarDecimal(dados.ValorVenda));
            eImovel.AdicionarElemento("PrecoLocacao", FormatarDecimal(dados.ValorLocacao));
            eImovel.AdicionarElemento("PrecoCondominio", FormatarDecimal(dados.ValorCondominio));
            eImovel.AdicionarElemento("ValorIPTU", FormatarDecimal(dados.ValorIPTU));
        }

        private static void AdicionarEndereco(DadosPrincipais dados, ElementoImovel eImovel)
        {
            string bairro = UsaZonaDeValor(dados.Estado)
                ? dados.Bairro
                : string.IsNullOrEmpty(dados.ZonaValor)
                    ? dados.Bairro
                    : dados.ZonaValor;

            eImovel.AdicionarElemento("UF", dados.Estado, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Cidade", dados.Cidade, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Bairro", bairro, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Endereco", dados.Logradouro, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Endereco", dados.CEP, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Numero", DefinirNumeroEndereco(dados), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Complemento", DefinirComplementoEndereco(dados), naoAdicionarSeNuloOuVazio: true);

            if (dados.Latitude.HasValue)
                eImovel.AdicionarElemento("Latitude", dados.Latitude.Value.ToString());
            if (dados.Longitude.HasValue)
                eImovel.AdicionarElemento("Longitude", dados.Longitude.Value.ToString());
        }

        private static string DefinirComplementoEndereco(DadosPrincipais dados)
        {
            return string.IsNullOrEmpty(dados.Complemento) ? "n/a" : dados.Complemento;
        }
        private static string DefinirNumeroEndereco(DadosPrincipais dados)
        {
            return !string.IsNullOrEmpty(dados.Numero) ? dados.Numero : "0";
        }

        private int DefinirQtdeComodos(int? qtdeComodo, string tipoImovel)
        {
            if (Terreno(tipoImovel))
                return 0;

            if (!qtdeComodo.HasValue || qtdeComodo.Value == 0)
                return 1;

            return qtdeComodo.Value;
        }

        private static void AdicionarVideos(DadosImovel dados, Elemento eImovel)
        {
            if (dados.UrlVideos.Any())
            {
                Elemento eVideos = eImovel.AdicionarElemento("Videos");
                foreach (string url in dados.UrlVideos)
                    eVideos.AdicionarElemento("Video", url);
            }
        }

        private static void AdicionarTourVirtuais(DadosImovel dados, Elemento eImovel)
        {
            if (dados.UrlTourVirtuais.Any())
            {
                foreach (string url in dados.UrlTourVirtuais)
                    eImovel.AdicionarElemento("LinkTourVirtual", url);
            }
        }

        private void AdicionarCaracteristicas(DadosImovel dados, Elemento eImovel)
        {
            int[] idCaracteristicasImovel = dados.Caracteristicas.Select(_ => _.Id).ToArray();

            IEnumerable<PortalCaracteristica> caracteristicas = PortalCaracteristicas.Where(_ => idCaracteristicasImovel.Contains(_.IdCaracteristica));
            foreach (string tag in caracteristicas.Select(_=> _.Tag))
                eImovel.AdicionarElemento(tag, "1");
        }

        private static bool Terreno(string descrition) => descrition.Contains("Terreno");


        private static string DeterminaSubTipoDoImovelParaTipoApartamento(string subtipo)
        {
            return (subtipo?.Trim()) switch
            {
                "Flat" or "Loft" => "Loft",
                "Kitnet" => "Kitchenette/Conjugados",
                _ => "Apartamento Padrão",
            };
        }
        private static string DeterminaCategoriaParaTipoApartamento(string? subtipo)
        {
            return subtipo switch
            {
                "Cobertura" => "Cobertura",
                "Cobertura Duplex" => "Cobertura Duplex",
                "Cobertura Triplex" => "Cobertura Triplex",
                "Duplex" => "Duplex",
                "Triplex" => "Triplex",
                _ => "Padrão",
            };
        }

        private static string DeterminaSubTipoDoImovelParaTipoCasa(string? subtipo)
        {
            return subtipo.Trim() switch
            {
                "Condomínio fechado" or "Condomínio" => "Casa de Condomínio",
                "Casa de vila" => "Casa de Vila",
                _ => "Casa Padrão",
            };
        }
        private static string DeterminaCategoriaParaTipoCasa(string? subtipo)
        {
            return subtipo.Trim() switch
            {
                "Condomínio fechado" or "Condomínio" => "Térrea",
                "Casa de vila" => "Térrea",
                "Assobradada" or "Cobertura" or "Sobrado" => "Sobrado/Duplex",
                _ => "Térrea",
            };
        }

        private static string DeterminaSubTipoDoImovelParaTipoComercial(string? subtipo)
        {
            return subtipo.Trim() switch
            {
                "Estúdio" or "Studio" => "Studio",
                "Galpão" => "Galpão/Depósito/Armazém",
                "Loja" => "Loja/Salão",
                "Prédio inteiro" => "Prédio Inteiro",
                "Sala" => "Conjunto Comercial/Sala",
                "Vaga" or "Vagas cobertas" => "Box/Garagem",
                _ => "Casa Comercial",
            };
        }
        private static string DeterminaCategoriaParaTipoComercial(string? subtipo)
        {
            return subtipo.Trim() switch
            {
                "Sobreposta" or "Duplex" or "Triplex" => "Sobrado/Duplex",
                "Casa" => "Térrea",
                _ => "Padrão",
            };
        }

        private static string DeterminaSubTipoDoImovelParaTipoPropriedadeRural(string? subtipo)
        {
            return subtipo switch
            {
                "Sítio" => "Sítio",
                "Chácara" => "Chácara",
                _ => "Fazenda",
            };
        }
        private static string DeterminaSubTipoDoImovelParaTipoTerreno(string subtipo)
        {
            return subtipo switch
            {
                "Loteamento" or "Condomínio fechado" => "Loteamento/Condomínio",
                _ => "Terreno Padrão",
            };
        }
        private static string DeterminaSubTipoDoImovelParaTipoHotel(string subtipo)
        {
            return subtipo.Trim() switch
            {
                "Pousada" => "Pousada/Chalé",
                _ => "Hotel",
            };
        }

        private void DeterminaTipoSubtipoCategoriaDoImovel(DadosPrincipais imovel, out string tipo, out string subtipo, out string categoria)
        {
            if (imovel.Subtipo == "Estúdio" || imovel.Subtipo == "Studio")//Alterado porque Zap não tem studio abaixo de apartamento
            {
                imovel.Tipo = "Comercial";
            }

            switch (imovel.Tipo?.Trim())
            {
                case "Casa":
                    tipo = imovel.Tipo.Trim();
                    subtipo = DeterminaSubTipoDoImovelParaTipoCasa(imovel.Subtipo);
                    categoria = DeterminaCategoriaParaTipoCasa(imovel.Subtipo);
                    break;
                case "Galpão":
                    tipo = "Comercial/Industrial";
                    subtipo = "Galpão";
                    categoria = "Padrão";
                    break;
                case "Hotel":
                    tipo = "Comercial/Industrial";
                    subtipo = DeterminaSubTipoDoImovelParaTipoHotel(imovel.Subtipo ?? string.Empty);
                    categoria = "Padrão";
                    break;
                case "Prédio":
                    tipo = "Comercial/Industrial";
                    subtipo = "Prédio Inteiro";
                    categoria = "Padrão";
                    break;
                case "Comercial":
                case "Vaga":
                case "Vaga de Garagem":
                case "Lajes Corporativas":
                case "Mall":
                case "Salas":
                    tipo = "Comercial/Industrial";
                    subtipo = DeterminaSubTipoDoImovelParaTipoComercial(imovel.Subtipo);
                    categoria = DeterminaCategoriaParaTipoComercial(imovel.Subtipo);
                    break;
                case "Propriedade Rural":
                    tipo = "Rural";
                    subtipo = DeterminaSubTipoDoImovelParaTipoPropriedadeRural(imovel.Subtipo);
                    categoria = "Padrão";
                    break;
                case "Loteamento":
                case "Terreno":
                    tipo = "Terreno";
                    subtipo = DeterminaSubTipoDoImovelParaTipoTerreno(imovel.Subtipo ?? string.Empty);
                    categoria = "Padrão";
                    break;
                case "Flat":
                    tipo = "Flat/Aparthotel";
                    subtipo = "Flat";
                    categoria = "Padrão";
                    break;
                case "Apartamento":
                default:
                    tipo = "Apartamento";
                    subtipo = DeterminaSubTipoDoImovelParaTipoApartamento(imovel.Subtipo ?? string.Empty);
                    categoria = DeterminaCategoriaParaTipoApartamento(imovel.Subtipo);
                    break;
            }
        }
    }
}