using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Lopes.Jobs.Api.Log;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Lopes.Anuncio.Domain.Commands.Requests;
using Hangfire.Storage;

namespace Lopes.Jobs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnuncioController : ControllerBase
    {

        private readonly ILogger<AnuncioController> _logger;
        private readonly IAtualizacaoAppService _service;

        public AnuncioController(ILogger<AnuncioController> logger, IAtualizacaoAppService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("ObterStatusJob")]
        public string ObterStatusJob(int idJob)
        {
            IStorageConnection connection = JobStorage.Current.GetConnection();
            JobData jobData = connection.GetJobData(idJob.ToString());
            string stateName = jobData.State;

            return stateName;
        }

        [HttpGet]
        [Route("AtualizarPorProduto")]
        public string AtualizarPorProduto([FromQuery] int[] idProdutos)
        {
            string? jobId = BackgroundJob.Enqueue(() => AtualizarPorProdutos(idProdutos, null));

            string mensagem = $"Atualiza??o para os im?veis '{string.Join(", ", idProdutos)}' enfileirada. JobId: {jobId}";

            _logger.Log(LogLevel.Information, mensagem);

            return mensagem;
        }

        [HttpGet]
        [Route("AtualizarPorProdutoPortal")]
        public ActionResult AtualizarPorProdutoPortal([FromQuery] int[] idProdutos, [FromQuery] Portal portal)
        {
            if (idProdutos == null || idProdutos.Length == 0)
            {
                return BadRequest("Necess?rio informar o(s) produtos e ou Portal.");
            }

            string? jobId = BackgroundJob.Enqueue(() => AtualizarPorProdutos(idProdutos, null, portal));

            string mensagem = $"Atualiza??o para os im?veis '{string.Join(", ", idProdutos)}' enfileirada. JobId: {jobId}";

            _logger.Log(LogLevel.Information, mensagem);

            return Ok(mensagem);
        }

        [HttpGet]
        [Route("AtualizarPorCota")]
        public JsonResult AtualizarPorCota([FromQuery] int[] idCotas)
        {
            string? idJob = BackgroundJob.Enqueue(() => AtualizarPorCotas(idCotas, null));

            string mensagem = $"Atualiza??o para a(s) cota(s) '{string.Join(", ", idCotas)}' enfileirada.";

            _logger.Log(LogLevel.Information, mensagem);

            return new JsonResult(new { idJob, mensagem });
        }

        [HttpGet]
        [Route("AtualizarPorPortal")]
        public string AtualizarPorPortal([FromQuery] Portal[] portais)
        {
            string? jobId2 = BackgroundJob.Enqueue(() => AtualizarPorPortal(portais, null));

            return $"Atualiza??o para os portais '{string.Join(", ", portais)}' enfileirada.";
        }




        [ApiExplorerSettings(IgnoreApi = true)]
        public void AtualizarPorPortal([FromQuery] Portal[] portal, PerformContext context)
        {
            var log = new HangFireLog(context);
            _service.AtualizarAnuncios(new AnuncioCotaRequest(portal), log);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void AtualizarPorCotas([FromQuery] int[] cotas, PerformContext context)
        {
            var log = new HangFireLog(context);
            _service.AtualizarAnuncios(new AnuncioCotaRequest(idCotas: cotas), log);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void AtualizarPorProdutos([FromQuery] int[] idProdutos, PerformContext context, Portal portal)
        {
            var log = new HangFireLog(context);
            _service.AtualizarAnuncios(new AnuncioCotaRequest(idProdutos: idProdutos, portal: new[] { portal }), log);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void AtualizarPorProdutos([FromQuery] int[] idProdutos, PerformContext context)
        {
            var log = new HangFireLog(context);
            _service.AtualizarAnuncios(new AnuncioCotaRequest(idProdutos: idProdutos), log);
        }


        [HttpGet]
        public string Teste()
        {

            var jobId = BackgroundJob.Enqueue(() => TaskMethod(null));
            //var jobId2 = BackgroundJob.Schedule(() => Console.WriteLine("Job 2"), TimeSpan.FromSeconds(60));
            return "!a";
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public void TaskMethod(PerformContext context)
        {
            _logger.Log(LogLevel.Warning, "Teste methodddd .");
            var progresso = context.WriteProgressBar();
            //var progresso2 = context.WriteProgressBar();
            progresso.SetValue(10);
            //progresso2.SetValue(10);

            context.WriteLine("Hello, world!");
            Thread.Sleep(5000);
            progresso.SetValue(20);
            //progresso2.SetValue(30);

            context.WriteLine("Hello, world! 2 ");
            Thread.Sleep(5000);
            progresso.SetValue(50);
            //progresso2.SetValue(80);

            context.WriteLine("Hello, world! 3");
            Thread.Sleep(5000);
            progresso.SetValue(90);

            context.WriteLine("Hello, world! 4");
            Thread.Sleep(5000);
            progresso.SetValue(95);

            context.WriteLine("Hello, world! 5");
            Thread.Sleep(5000);
            progresso.SetValue(98);

            context.WriteLine("Hello, world! 6");
            Thread.Sleep(5000);
            progresso.SetValue(100);
            //progresso2.SetValue(100);

            //context.WriteLine("Conclu?do");
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<AnuncioAtualizacao> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new AnuncioAtualizacao
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}