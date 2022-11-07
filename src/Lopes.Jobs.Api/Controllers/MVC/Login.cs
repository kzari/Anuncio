using Lopes.Acesso.Application.Services;
using Lopes.Acesso.Domain.Commands.Requests;
using Lopes.Acesso.Domain.Commands.Responses;
using Lopes.Jobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Api.Controllers.MVC
{
    public class Login : Controller
    {
        private readonly IUsuarioAcessoAppService _usuarioAcessoAppService;

        public Login(UsuarioAcessoAppService usuarioAcessoAppService)
        {
            _usuarioAcessoAppService = usuarioAcessoAppService;
        }

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
                if(!ModelState.IsValid)
                    return View(model);

                string token = _usuarioAcessoAppService.ObterTokenAutenticado(model.Usuario, model.Senha);
                if(!string.IsNullOrEmpty(token))
                {
                    //Add token to cookie?

                    return RedirectToAction("Index", "");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
