using Lopes.Anuncio.Domain.Enums;
using Lopes.Anuncio.Domain.Models.DadosProduto;
using Lopes.Anuncio.Domain.ObjetosValor;
using Lopes.Anuncio.Domain.Services;
using Lopes.Anuncio.Domain.XML;

namespace Lopes.Anuncio.Domain.Models.XML.Portais
{
    public class Zap : PortalXmlElementosBase, IPortalXMLElementos
    {
        public Zap(Portal portal, IEnumerable<PortalCaracteristica> portalCaracteristicas, string urlFotosProdutos) : base(portal, portalCaracteristicas, urlFotosProdutos)
        {
        }

        public string CaminhoTagPaiProdutos => "/Carga/Produtos";
        public string NomeTagImovel => "Imovel";
        public string NomeTagCodigoImovel => "CodigoImovel";

        protected override Elemento CriarElementoCabecalho()
        {
            var eCarga = new Elemento("Carga", null, new List<Atributo>
            {
                new Atributo("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new Atributo("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
            });

            eCarga.AdicionarFilho(nome: "Produtos");

            return eCarga;
        }

        protected override ElementoProduto CriarElementoImovel(Produto dados)
        {
            ElementoProduto eImovel = new ElementoProduto(dados.Dados.IdProduto, "Imovel");

            DeterminaTipoSubtipoCategoriaDoImovel(dados.Dados, out string tipoImovel, out string subTipoImovel, out string categoria);

            eImovel.AdicionarFilho("CodigoImovel", dados.Dados.IdImovelPortais);

            eImovel.AdicionarFilho("TipoImovel", dados.Dados.Tipo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("SubTipoImovel", dados.Dados.Subtipo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("TituloImovel", dados.Dados.Titulo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Observacao", RemoverCaracteresInvalidosUnicode(dados.Dados.TextoSite, " "), naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarFilho("InscricaoMunicipal", dados.Dados.InscricaoMunicipal, naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarFilho("QtdDormitorios", DefinirQtdeComodos(dados.Dados.QtdeQuartos, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("QtdSuites", DefinirQtdeComodos(dados.Dados.QtdeSuites, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("QtdBanheiros", DefinirQtdeComodos(dados.Dados.QtdeBanheirosSociais, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("QtdVagas", DefinirQtdeComodos(dados.Dados.QtdeVagas, tipoImovel).ToString(), naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarFilho("AnoConstrucao", dados.Dados.AnoConstrucao?.ToString(), naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarFilho("AreaUtil", FormatarDecimal(dados.Dados.AreaPrivativa), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("AreaTotal", FormatarDecimal(dados.Dados.AreaTotal), naoAdicionarSeNuloOuVazio: true);

            AdicionarEndereco(dados.Dados, eImovel);

            AdicionarValores(dados.Dados, eImovel);

            AdicionarVideos(dados, eImovel);

            AdicionarTourVirtuais(dados, eImovel);

            AdicionarCaracteristicas(dados, eImovel);

            AdicionarFotos(dados, eImovel);

            return eImovel;
        }

        private void AdicionarFotos(Produto dados, ElementoProduto eImovel)
        {
            if (!dados.Imagens.Any())
                return;

            Elemento eFotos = eImovel.AdicionarFilho("Fotos");
            foreach (Foto foto in dados.Imagens)
            {
                Elemento eFoto = eFotos.AdicionarFilho("Foto");
                eFoto.AdicionarFilho("NomeArquivo", foto.Descricao);
                eFoto.AdicionarFilho("Principal", foto.Ordem == 1 ? "1" : "0");
                eFoto.AdicionarFilho("URLArquivo", foto.ObterCaminhoFotoImovel(UrlFotosProdutos));
                eFoto.AdicionarFilho("Alterada", "1");
            }
        }

        private static void AdicionarValores(DadosPrincipais dados, ElementoProduto eImovel)
        {
            eImovel.AdicionarFilho("PrecoVenda", FormatarDecimal(dados.ValorVenda));
            eImovel.AdicionarFilho("PrecoLocacao", FormatarDecimal(dados.ValorLocacao));
            eImovel.AdicionarFilho("PrecoCondominio", FormatarDecimal(dados.ValorCondominio));
            eImovel.AdicionarFilho("ValorIPTU", FormatarDecimal(dados.ValorIPTU));
        }

        private static void AdicionarEndereco(DadosPrincipais dados, ElementoProduto eImovel)
        {
            string bairro = UsaZonaDeValor(dados.Estado)
                ? dados.Bairro
                : string.IsNullOrEmpty(dados.ZonaValor)
                    ? dados.Bairro
                    : dados.ZonaValor;

            eImovel.AdicionarFilho("UF", dados.Estado, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Cidade", dados.Cidade, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Bairro", bairro, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Endereco", dados.Logradouro, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Endereco", dados.CEP, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Numero", DefinirNumeroEndereco(dados), naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarFilho("Complemento", DefinirComplementoEndereco(dados), naoAdicionarSeNuloOuVazio: true);

            if (dados.Latitude.HasValue)
                eImovel.AdicionarFilho("Latitude", dados.Latitude.Value.ToString());
            if (dados.Longitude.HasValue)
                eImovel.AdicionarFilho("Longitude", dados.Longitude.Value.ToString());
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

        private static void AdicionarVideos(Produto dados, Elemento eImovel)
        {
            if (dados.UrlVideos.Any())
            {
                Elemento eVideos = eImovel.AdicionarFilho("Videos");
                foreach (string url in dados.UrlVideos)
                    eVideos.AdicionarFilho("Video", url);
            }
        }

        private static void AdicionarTourVirtuais(Produto dados, Elemento eImovel)
        {
            if (dados.UrlTourVirtuais.Any())
            {
                foreach (string url in dados.UrlTourVirtuais)
                    eImovel.AdicionarFilho("LinkTourVirtual", url);
            }
        }

        private void AdicionarCaracteristicas(Produto dados, Elemento eImovel)
        {
            int[] idCaracteristicasImovel = dados.Caracteristicas.Select(_ => _.Id).ToArray();

            IEnumerable<PortalCaracteristica> caracteristicas = PortalCaracteristicas.Where(_ => idCaracteristicasImovel.Contains(_.IdCaracteristica));
            foreach (string tag in caracteristicas.Select(_ => _.Tag))
                eImovel.AdicionarFilho(tag, "1");
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