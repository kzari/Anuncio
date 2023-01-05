using Lopes.Botmaker.Application.Models;
using Lopes.Botmaker.Application.Services;
using Lopes.Domain.Commons.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Web.Controllers;

public class BotmakerController : Controller
{
    private readonly IIntegracaoAppService _service;
    private readonly ICacheService _cache;

    public BotmakerController(IIntegracaoAppService service, ICacheService cache)
    {
        _service = service;
        _cache = cache;
    }


    public IActionResult Index(bool ignorarCache = false)
    {
        List<UsuarioIntegracao> usuarios;
        if (ignorarCache)
        {
            usuarios = ObterUsuarios();
        }
        else
        {
            usuarios = _cache.ObterOuGravar("UsuariosBotmaker", TimeSpan.FromMinutes(30), ObterUsuarios).ToList();
        }
        return View(usuarios);
    }


    private List<UsuarioIntegracao> ObterUsuarios()
    {
        return _service.ObterUsuarios().ToList().OrderBy(_ => _.Acao == "Atualizado" ? 1 : 0).ToList();
    }
}
