using Lopes.Anuncio.Application.Models;
using Lopes.Anuncio.Application.Services;
using Lopes.Domain.Commons.Cache;
using Lopes.Jobs.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lopes.Jobs.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICotaAppService _cotaService;
        private readonly ICacheService _cacheService;

        public HomeController(ILogger<HomeController> logger, 
                              ICotaAppService cotaService, 
                              ICacheService cacheService)
        {
            _logger = logger;
            _cotaService = cotaService;
            _cacheService = cacheService;
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
            IEnumerable<CotaResumoViewModel> cotas = _cacheService.ObterOuGravar("Cotas", TimeSpan.FromMinutes(30), _cotaService.ObterCotas);

            AnunciosResumoViewModel resumo = new(cotas.ToList());

            return new JsonResult(resumo);
        }
    }
}