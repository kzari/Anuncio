using Lopes.Botmaker.Application.Models;
using Lopes.Botmaker.Application.Services;
using Lopes.Domain.Commons;
using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Web.Controllers;

public class BotmakerController : Controller
{
    private readonly IIntegracaoAppService _service;

    public BotmakerController(IIntegracaoAppService service)
    {
        _service = service;
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


    private List<UsuarioIntegracao> ObterUsuarios(bool ignorarCache)
    {
        List<UsuarioIntegracao> usuarios = ignorarCache
            ? _service.ObterUsuarios().ToList()
            : _service.ObterUsuarios(duracaoCacheBotmaker: TimeSpan.FromMinutes(15), duracaoCacheBd: TimeSpan.FromHours(1)).ToList();

        return usuarios.OrderBy(_ => _.Acao == "Atualizado" ? 1 : 0)
                       .ToList();
    }
}
