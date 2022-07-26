using Lopes.SC.ExportacaoAnuncio.Domain.Imovel;
using Lopes.SC.ExportacaoAnuncio.Domain.Services;

namespace Lopes.SC.ExportacaoAnuncio.Domain.XML
{
    public class Zap : PortalXmlElementosBase, IPortalXMLElementos
    {
        //TODO: para interface
        protected override string CaminhoTagPaiImoveis => "/Carga/Imoveis";


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

            eImovel.AdicionarElemento("CodigoImovel", dados.Dados.IdImovel.ToString());

            eImovel.AdicionarElemento("TipoImovel", dados.Dados.Tipo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("SubTipoImovel", dados.Dados.Subtipo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("TituloImovel", dados.Dados.Titulo, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Observacao", dados.Dados.TextoSite, naoAdicionarSeNuloOuVazio: true);

            eImovel.AdicionarElemento("UF", dados.Dados.Estado, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Cidade", dados.Dados.Cidade, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Bairro", dados.Dados.Bairro, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("Endereco", dados.Dados.Logradouro, naoAdicionarSeNuloOuVazio: true);
            eImovel.AdicionarElemento("CEP", dados.Dados.CEP, naoAdicionarSeNuloOuVazio: true);

            if (dados.Dados.Latitude.HasValue)
                eImovel.AdicionarElemento("Latitude", dados.Dados.Latitude.Value.ToString());
            if (dados.Dados.Longitude.HasValue)
                eImovel.AdicionarElemento("Longitude", dados.Dados.Longitude.Value.ToString());

            AdicionarVideos(dados, eImovel);


            return eImovel;
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
    }
}