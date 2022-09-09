using Hangfire;
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Enums;

namespace Lopes.Jobs.Api
{
    public class Startup
    {


        public void Configure(IApplicationBuilder applicationBuilder)
        {

            var log = applicationBuilder.ApplicationServices.GetService<Domain.Commons.ILogger>();
            RecurringJob.AddOrUpdate<IAtualizarAnunciosAppService>(x => x.AtualizarPorPortais(new[] { Portal.Zap }, log), Cron.Daily);
        }
    }
}
