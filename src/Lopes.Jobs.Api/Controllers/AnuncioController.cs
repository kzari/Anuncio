using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Lopes.Jobs.Api.Log;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Lopes.Anuncio.Domain.Commands.Requests;

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
        [Route("AtualizarPorImovel")]
        public string AtualizarPorImovel([FromQuery] int[] idProdutos)
        {
            string? jobId = BackgroundJob.Enqueue(() => AtualizarPorProdutos(idProdutos, null, null));

            string mensagem = $"Atualização para os imóveis '{string.Join(", ", idProdutos)}' enfileirada. JobId: {jobId}";

            _logger.Log(LogLevel.Information, mensagem);

            return mensagem;
        }

        [HttpGet]
        [Route("AtualizarPorImovelPortal")]
        public string AtualizarPorImovelPortal([FromQuery] int[] idProdutos, [FromQuery] Portal portal)
        {
            string? jobId = BackgroundJob.Enqueue(() => AtualizarPorProdutos(idProdutos, null, portal));

            string mensagem = $"Atualização para os imóveis '{string.Join(", ", idProdutos)}' enfileirada. JobId: {jobId}";

            _logger.Log(LogLevel.Information, mensagem);

            return mensagem;
        }

        [HttpGet]
        [Route("AtualizarPorCota")]
        public string AtualizarPorCota([FromQuery] int[] idCotas)
        {
            string? jobId = BackgroundJob.Enqueue(() => AtualizarPorCotas(idCotas, null));

            string mensagem = $"Atualização para os portais '{string.Join(", ", idCotas)}' enfileirada. JobId: {jobId}";

            _logger.Log(LogLevel.Information, mensagem);

            return mensagem;
        }

        [HttpGet]
        [Route("AtualizarPorPortal")]
        public string AtualizarPorPortal([FromQuery] Portal[] portais)
        {
            string? jobId2 = BackgroundJob.Enqueue(() => AtualizarPorPortal(portais, null));

            return $"Atualização para os portais '{string.Join(", ", portais)}' enfileirada.";
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
        public void AtualizarPorProdutos([FromQuery] int[] idProdutos, PerformContext context, Portal? portal = null)
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

            //context.WriteLine("Concluído");
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