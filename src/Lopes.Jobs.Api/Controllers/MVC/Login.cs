using Lopes.Jobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Api.Controllers.MVC
{
    public class Login : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginModel model)
        {
            try
            {

                return RedirectToAction("Index", "");
            }
            catch
            {
                return View();
            }
        }
    }
}
