using Lopes.Acesso.App.Services;
using Lopes.Jobs.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioAcessoAppService _usuarioAcessoAppService;

        public LoginController(IUsuarioAcessoAppService usuarioAcessoAppService)
        {
            _usuarioAcessoAppService = usuarioAcessoAppService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var login = _usuarioAcessoAppService.ObterTokenAutenticado(model.Usuario, model.Senha);
            if (!string.IsNullOrEmpty(login.Erro))
            {
                //TODO: Add token to cookie?
                //TODO: Mostrar erro
                ViewBag.Erro = login.Erro;

                return View(model);
            }

            return RedirectToAction("Index", "");
        }
    }
}