using RestSharp;

namespace Julio.Jobs.Web.Utils
{
    /// <summary>
    /// Classe para consumo da api Jobs
    /// </summary>
    public class ApiJobs
    {
        public ApiJobs(IConfiguration configuration)
        {
            string urlApi = configuration["UrlApi"];

            Botmaker = new Botmaker(urlApi);
            Anuncio = new Anuncio(urlApi);
        }

        public Botmaker Botmaker { get; }
        public Anuncio Anuncio { get; }
    }

    public class ApiControllerBase
    {
        protected readonly string _urlApi;

        public ApiControllerBase(string urlApi)
        {
            _urlApi = urlApi;
            Controller = this.GetType().Name;
        }

        public string Controller { get; }

        protected static RestResponse ExecutarRest(string nomeMetodo)
        {
            //var url = _urlApi + 
            //var client = new RestClient(url);
            //RestResponse response = client.Execute(new RestRequest());
            return response;
        }
    }

    public class Botmaker : ApiControllerBase
    {
        public Botmaker(string urlApi) : base(urlApi)
        {
        }

        public RestResponse Integrar()
        {
            string url = _urlApi + "Botmaker/Integrar";
            
            return ExecutarRest(url);
        }
    }

    public class Anuncio : ApiControllerBase
    {
        public Anuncio(string urlApi) : base(urlApi)
        {
        }

        public RestResponse AtualizarPorProduto(int[] idProdutos)
        {
            string url = _urlApi + $"Anuncio/AtualizarPorProduto?idProdutos={idProdutos}";
            var client = new RestClient(url);
            RestResponse response = client.Execute(new RestRequest());
            return response;
        }
    }
}
