namespace Lopes.Jobs.Api.Configuracoes
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
