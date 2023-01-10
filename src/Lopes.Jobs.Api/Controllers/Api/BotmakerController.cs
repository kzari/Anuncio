using Hangfire;
using Hangfire.Server;
using Lopes.Botmaker.Application.Services;
using Lopes.Jobs.Api.Log;
using Microsoft.AspNetCore.Mvc;

namespace Lopes.Jobs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotmakerController : ControllerBase
    {
        private readonly ILogger<AnuncioController> _logger;
        private readonly IIntegracaoAppService _service;

        public BotmakerController(ILogger<AnuncioController> logger, IIntegracaoAppService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpGet]
        [Route("Integrar")]
        public string Integrar()
        {
            string? jobId = BackgroundJob.Enqueue(() => Integrar(null));

            string mensagem = $"Integração com a Botmaker enfileirada. JobId: {jobId}";

            _logger.Log(LogLevel.Information, mensagem);

            return mensagem;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public void Integrar(PerformContext context)
        {
            var log = new HangFireLog(context);

            _service.IntegrarUsuarios(log);
        }
    }
}