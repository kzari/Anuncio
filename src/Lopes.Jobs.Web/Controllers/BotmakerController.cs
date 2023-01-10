using Lopes.Botmaker.Application.Models;
using Lopes.Botmaker.Application.Services;
using Lopes.Domain.Commons;
using Lopes.Jobs.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Lopes.Jobs.Web.Controllers;

public class BotmakerController : Controller
{
    private readonly IIntegracaoAppService _service;
    private readonly IConfiguration _configuration;
    private readonly ApiJobs _api;

    public BotmakerController(IIntegracaoAppService service,
                              IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
        _api = new ApiJobs();
    }


    public IActionResult Index(bool ignorarCache = false)
    {
        List<UsuarioIntegracao> usuarios = ObterUsuarios(ignorarCache);
        return View(usuarios);
    }

    public IActionResult EnviarUsuario(string email)
    {
        IResultadoItens resultado = _service.EnviarUsuarios(new[] { email });
        return Json(resultado);
    }
    public IActionResult Integrar()
    {
        RestResponse response = _api.Botmaker.Integrar();

        return Json(response.Content);
    }


    private List<UsuarioIntegracao> ObterUsuarios(bool ignorarCache)
    {
        List<UsuarioIntegracao> usuarios = ignorarCache
            ? _service.ObterUsuarios().ToList()
            : _service.ObterUsuarios(duracaoCacheBotmaker: TimeSpan.FromMinutes(15), duracaoCacheBd: TimeSpan.FromHours(1)).ToList();

        return usuarios.OrderBy(_ => _.Acao == "Atualizado" ? 1 : 0)
                       .ToList();
    }
}
