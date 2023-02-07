using Hangfire;
using Julio.Anuncio.Application.Interfaces;
using Julio.Anuncio.Domain.Commands.Requests;
using Julio.Anuncio.Domain.Enums;

namespace Julio.Jobs.Api
{
    public class Startup
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            Domain.Commons.ILogger? logger = applicationBuilder.ApplicationServices.GetService<Domain.Commons.ILogger>();
            if (logger == null)
                throw new Exception("serviço de log não encontrado");

            //RecurringJob.AddOrUpdate<IAtualizacaoAppService>(x => x.AtualizarAnuncios(new AnuncioCotaRequest(Portal.Zap), logger), Cron.Daily);
        }
    }
}
