﻿using Hangfire;
using Lopes.SC.Anuncio.Application.Interfaces;
using Lopes.SC.Anuncio.Domain.Enums;

namespace Lopes.SC.Jobs.Api
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