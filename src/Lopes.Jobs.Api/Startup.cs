using Hangfire;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Jobs.Api
{
    public class Startup
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            Domain.Commons.ILogger? logger = applicationBuilder.ApplicationServices.GetService<Domain.Commons.ILogger>();
            if (logger == null)
                throw new Exception("serviço de log não encontrado");

            RecurringJob.AddOrUpdate<IAtualizacaoAppService>(x => x.Atualizar(new AnuncioCotaRequest(Portal.Zap), logger), Cron.Daily);
        }
    }
}
