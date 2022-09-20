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

        public string CaminhoTagPaiProdutos => "/Carga/Imoveis";
        public string NomeTagProduto => "Imovel";
        public string NomeTagCodigoProduto => "CodigoImovel";

        protected override Elemento CriarElementoCabecalho()
        {
            var eCarga = new Elemento("Carga", null, new List<Atributo>
            {
                new Atributo("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new Atributo("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
            });

            eCarga.AdicionarFilho(nome: "Imoveis");

            return eCarga;
        }

        protected override ElementoProduto CriarElementoProduto(Produto dados)
        {
            ElementoProduto eProduto = new ElementoProduto(dados.Dados.IdProduto, NomeTagProduto);

            DeterminaTipoSubtipoCategoriaDoProduto(dados.Dados, out string tipoProduto, out string subTipoProduto, out string categoria);

            eProduto.AdicionarFilho(NomeTagCodigoProduto, dados.Dados.IdProdutoPortais);

            eProduto.AdicionarFilho("TipoImovel", dados.Dados.Tipo, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("SubTipoImovel", dados.Dados.Subtipo, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("TituloImovel", dados.Dados.Titulo, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Observacao", RemoverCaracteresInvalidosUnicode(dados.Dados.TextoSite, " "), naoAdicionarSeNuloOuVazio: true);

            eProduto.AdicionarFilho("InscricaoMunicipal", dados.Dados.InscricaoMunicipal, naoAdicionarSeNuloOuVazio: true);

            eProduto.AdicionarFilho("QtdDormitorios", DefinirQtdeComodos(dados.Dados.QtdeQuartos, tipoProduto).ToString(), naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("QtdSuites", DefinirQtdeComodos(dados.Dados.QtdeSuites, tipoProduto).ToString(), naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("QtdBanheiros", DefinirQtdeComodos(dados.Dados.QtdeBanheirosSociais, tipoProduto).ToString(), naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("QtdVagas", DefinirQtdeComodos(dados.Dados.QtdeVagas, tipoProduto).ToString(), naoAdicionarSeNuloOuVazio: true);

            eProduto.AdicionarFilho("AnoConstrucao", dados.Dados.AnoConstrucao?.ToString(), naoAdicionarSeNuloOuVazio: true);

            eProduto.AdicionarFilho("AreaUtil", FormatarDecimal(dados.Dados.AreaPrivativa), naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("AreaTotal", FormatarDecimal(dados.Dados.AreaTotal), naoAdicionarSeNuloOuVazio: true);

            AdicionarEndereco(dados.Dados, eProduto);

            AdicionarValores(dados.Dados, eProduto);

            AdicionarVideos(dados, eProduto);

            AdicionarTourVirtuais(dados, eProduto);

            AdicionarCaracteristicas(dados, eProduto);

            AdicionarFotos(dados, eProduto);

            return eProduto;
        }

        private void AdicionarFotos(Produto dados, ElementoProduto eProduto)
        {
            if (!dados.Fotos.Any())
                return;

            Elemento eFotos = eProduto.AdicionarFilho("Fotos");
            foreach (Foto foto in dados.Fotos)
            {
                Elemento eFoto = eFotos.AdicionarFilho("Foto");
                eFoto.AdicionarFilho("NomeArquivo", foto.Descricao);
                eFoto.AdicionarFilho("Principal", foto.Ordem == 1 ? "1" : "0");
                eFoto.AdicionarFilho("URLArquivo", foto.ObterCaminhoFotoProduto(UrlFotosProdutos));
                eFoto.AdicionarFilho("Alterada", "1");
            }
        }

        private static void AdicionarValores(DadosPrincipais dados, ElementoProduto eProduto)
        {
            eProduto.AdicionarFilho("PrecoVenda", FormatarDecimal(dados.ValorVenda));
            eProduto.AdicionarFilho("PrecoLocacao", FormatarDecimal(dados.ValorLocacao));
            eProduto.AdicionarFilho("PrecoCondominio", FormatarDecimal(dados.ValorCondominio));
            eProduto.AdicionarFilho("ValorIPTU", FormatarDecimal(dados.ValorIPTU));
        }

        private static void AdicionarEndereco(DadosPrincipais dados, ElementoProduto eProduto)
        {
            string bairro = UsaZonaDeValor(dados.Estado)
                ? dados.Bairro
                : string.IsNullOrEmpty(dados.ZonaValor)
                    ? dados.Bairro
                    : dados.ZonaValor;

            eProduto.AdicionarFilho("UF", dados.Estado, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Cidade", dados.Cidade, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Bairro", bairro, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Endereco", dados.Logradouro, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Endereco", dados.CEP, naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Numero", DefinirNumeroEndereco(dados), naoAdicionarSeNuloOuVazio: true);
            eProduto.AdicionarFilho("Complemento", DefinirComplementoEndereco(dados), naoAdicionarSeNuloOuVazio: true);

            if (dados.Latitude.HasValue)
                eProduto.AdicionarFilho("Latitude", dados.Latitude.Value.ToString());
            if (dados.Longitude.HasValue)
                eProduto.AdicionarFilho("Longitude", dados.Longitude.Value.ToString());
        }

        private static string DefinirComplementoEndereco(DadosPrincipais dados)
        {
            return string.IsNullOrEmpty(dados.Complemento) ? "n/a" : dados.Complemento;
        }
        private static string DefinirNumeroEndereco(DadosPrincipais dados)
        {
            return !string.IsNullOrEmpty(dados.Numero) ? dados.Numero : "0";
        }

        private int DefinirQtdeComodos(int? qtdeComodo, string tipoProduto)
        {
            if (Terreno(tipoProduto))
                return 0;

            if (!qtdeComodo.HasValue || qtdeComodo.Value == 0)
                return 1;

            return qtdeComodo.Value;
        }

        private static void AdicionarVideos(Produto dados, Elemento eProduto)
        {
            if (dados.UrlVideos.Any())
            {
                Elemento eVideos = eProduto.AdicionarFilho("Videos");
                foreach (string url in dados.UrlVideos)
                    eVideos.AdicionarFilho("Video", url);
            }
        }

        private static void AdicionarTourVirtuais(Produto dados, Elemento eProduto)
        {
            if (dados.UrlTourVirtuais.Any())
            {
                foreach (string url in dados.UrlTourVirtuais)
                    eProduto.AdicionarFilho("LinkTourVirtual", url);
            }
        }

        private void AdicionarCaracteristicas(Produto dados, Elemento eProduto)
        {
            int[] idCaracteristicasProduto = dados.Caracteristicas.Select(_ => _.Id).ToArray();

            IEnumerable<PortalCaracteristica> caracteristicas = PortalCaracteristicas.Where(_ => idCaracteristicasProduto.Contains(_.IdCaracteristica));
            foreach (string tag in caracteristicas.Select(_ => _.Tag))
                eProduto.AdicionarFilho(tag, "1");
        }

        private static bool Terreno(string descrition) => descrition.Contains("Terreno");


        private static string DeterminaSubTipoDoProdutoParaTipoApartamento(string subtipo)
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

        private static string DeterminaSubTipoDoProdutoParaTipoCasa(string? subtipo)
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

        private static string DeterminaSubTipoDoProdutoParaTipoComercial(string? subtipo)
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

        private static string DeterminaSubTipoDoProdutoParaTipoPropriedadeRural(string? subtipo)
        {
            return subtipo switch
            {
                "Sítio" => "Sítio",
                "Chácara" => "Chácara",
                _ => "Fazenda",
            };
        }
        private static string DeterminaSubTipoDoProdutoParaTipoTerreno(string subtipo)
        {
            return subtipo switch
            {
                "Loteamento" or "Condomínio fechado" => "Loteamento/Condomínio",
                _ => "Terreno Padrão",
            };
        }
        private static string DeterminaSubTipoDoProdutoParaTipoHotel(string subtipo)
        {
            return subtipo.Trim() switch
            {
                "Pousada" => "Pousada/Chalé",
                _ => "Hotel",
            };
        }

        private void DeterminaTipoSubtipoCategoriaDoProduto(DadosPrincipais imovel, out string tipo, out string subtipo, out string categoria)
        {
            if (imovel.Subtipo == "Estúdio" || imovel.Subtipo == "Studio")//Alterado porque Zap não tem studio abaixo de apartamento
            {
                imovel.Tipo = "Comercial";
            }

            switch (imovel.Tipo?.Trim())
            {
                case "Casa":
                    tipo = imovel.Tipo.Trim();
                    subtipo = DeterminaSubTipoDoProdutoParaTipoCasa(imovel.Subtipo);
                    categoria = DeterminaCategoriaParaTipoCasa(imovel.Subtipo);
                    break;
                case "Galpão":
                    tipo = "Comercial/Industrial";
                    subtipo = "Galpão";
                    categoria = "Padrão";
                    break;
                case "Hotel":
                    tipo = "Comercial/Industrial";
                    subtipo = DeterminaSubTipoDoProdutoParaTipoHotel(imovel.Subtipo ?? string.Empty);
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
                    subtipo = DeterminaSubTipoDoProdutoParaTipoComercial(imovel.Subtipo);
                    categoria = DeterminaCategoriaParaTipoComercial(imovel.Subtipo);
                    break;
                case "Propriedade Rural":
                    tipo = "Rural";
                    subtipo = DeterminaSubTipoDoProdutoParaTipoPropriedadeRural(imovel.Subtipo);
                    categoria = "Padrão";
                    break;
                case "Loteamento":
                case "Terreno":
                    tipo = "Terreno";
                    subtipo = DeterminaSubTipoDoProdutoParaTipoTerreno(imovel.Subtipo ?? string.Empty);
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
                    subtipo = DeterminaSubTipoDoProdutoParaTipoApartamento(imovel.Subtipo ?? string.Empty);
                    categoria = DeterminaCategoriaParaTipoApartamento(imovel.Subtipo);
                    break;
            }
        }
    }
}