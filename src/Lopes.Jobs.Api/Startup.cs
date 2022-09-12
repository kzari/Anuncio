using Hangfire;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Jobs.Api
{
    public class Startup
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            Domain.Commons.ILogger? log = applicationBuilder.ApplicationServices.GetService<Domain.Commons.ILogger>();
            if (log == null)
                throw new Exception("serviço de log não encontrado");

            RecurringJob.AddOrUpdate<IAtualizarAnunciosAppService>(x => x.AtualizarPorPortais(new[] { Portal.Zap }, log), Cron.Daily);
        }
    }
}
