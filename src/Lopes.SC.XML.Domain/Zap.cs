using Lopes.SC.AnuncioXML.Domain.Models;

namespace Lopes.SC.XML.Domain
{
    public class Zap : IPortalXML
    {
        public Elemento CriarElementoCabecalho()
        {
            var eCarga = new Elemento("Carga", null, new List<Atributo>
            {
                new Atributo("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new Atributo("xmlns:xsd", "http://www.w3.org/2001/XMLSchema")
            });

            eCarga.AdicionarElemento(nome: "Imoveis");

            return eCarga;
        }

        public Elemento CriarElementoImovel(Dados dados)
        {
            Elemento eImovel = new Elemento(nome: "Imovel");

            eImovel.AdicionarElemento("CodigoImovel", dados.IdImovelNoPortal);
            eImovel.AdicionarElemento("TipoImovel", dados.Tipo);
            eImovel.AdicionarElemento("SubTipoImovel", dados.Subtipo);
            eImovel.AdicionarElemento("TituloImovel", dados.Titulo);
            eImovel.AdicionarElemento("Observacao", dados.TextoSite);

            eImovel.AdicionarElemento("UF", dados.Estado);
            eImovel.AdicionarElemento("Cidade", dados.Cidade);
            eImovel.AdicionarElemento("Bairro", dados.Bairro);
            eImovel.AdicionarElemento("Endereco", dados.Logradouro);
            eImovel.AdicionarElemento("CEP", dados.CEP);

            if (dados.Latitude.HasValue)
                eImovel.AdicionarElemento("Latitude", dados.Latitude.Value.ToString(), naoAdicionarSeValorNulo: true);
            if (dados.Longitude.HasValue)
                eImovel.AdicionarElemento("Longitude", dados.Longitude.Value.ToString(), naoAdicionarSeValorNulo: true);

            AdicionarVideos(dados, eImovel);


            return eImovel;
        }

        private static void AdicionarVideos(Dados dados, Elemento eImovel)
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