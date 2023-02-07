using Julio.Anuncio.Application.Models;
using Julio.Anuncio.Application.Services;
using Julio.Domain.Commons.Cache;
using Julio.Jobs.Web.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;

namespace Julio.Jobs.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICotaAppService _cotaService;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger,
                              ICotaAppService cotaService,
                              ICacheService cacheService,
                              IConfiguration configuration)
        {
            _logger = logger;
            _cotaService = cotaService;
            _cacheService = cacheService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ObterResumoCotas()
        {
            //IEnumerable<CotaResumoViewModel> cotas =  _cotaService.ObterCotas().ToList();
            IEnumerable<CotaResumoViewModel> cotas = _cacheService.ObterOuGravar("Cotas", TimeSpan.FromMinutes(30), _cotaService.ObterCotas);

            //var algumaCotaInativa = cotas.Where(_=> !_.CotaAtiva && _.Portal == 68).ToList();

            AnunciosResumoViewModel resumo = new(cotas.ToList());

            return new JsonResult(resumo);
        }

        public JsonResult ObterAnunciosDesatualizados(int idPortal, bool ignorarCache = false)
        {
            string chaveCache = $"AnunciosDesatualizados_{idPortal}";
            AnunciosDesatualizadosViewModel anuncios;

            if (!ignorarCache)
            {
                anuncios = _cacheService.Obter<AnunciosDesatualizadosViewModel>(chaveCache);
                if (anuncios != null)
                {
                    return new JsonResult(anuncios);
                }
            }

            anuncios = _cotaService.ObterAnunciosDesatualizadosPorPortal(idPortal);
            _cacheService.Gravar(chaveCache, anuncios, TimeSpan.FromMinutes(30));

            return new JsonResult(anuncios);
        }

        public JsonResult ObterAnunciosDesatualizadosPorCota(int idCota)
        {
            AnunciosDesatualizadosViewModel anuncios = _cotaService.ObterAnunciosDesatualizadosPorCota(idCota);

            return new JsonResult(anuncios);
        }

        public JsonResult AtualizarCota(int idCota)
        {
            string url = _configuration["UrlApi"] + $"Anuncio/AtualizarPorCota?idCotas={idCota}";

            var client = new RestClient(url);
            RestResponse response = client.Execute(new RestRequest());

            return new JsonResult(response.Content);
        }

        public string ObterStatusJob(int idJob)
        {
            string url = _configuration["UrlApi"] + $"Anuncio/ObterStatusJob?idJob={idJob}";

            var client = new RestClient(url);
            RestResponse response = client.Execute(new RestRequest());

            return response?.Content.Replace("\"", "") ?? string.Empty;
        }
    }
}