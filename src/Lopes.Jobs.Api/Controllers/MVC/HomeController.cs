using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Api.Controllers.MVC
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
