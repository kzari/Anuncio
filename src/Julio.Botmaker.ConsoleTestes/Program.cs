using Julio.Acesso.ConsoleCommons.Log;
using Julio.Acesso.MemoryCache;
using Julio.Botmaker.Application.Services;
using Julio.Botmaker.IoC;
using Julio.Domain.Common.IoC;
using Julio.Domain.Commons.Cache;
using Julio.Infra.Common;
using Microsoft.Extensions.DependencyInjection;

ConfiguradorIoC ioc = ConfiguradorIoC.ConfigurarServicos<ConsoleLogger>(new BotmakerIoC(TipoBaseDados.Producao));

ioc.ServiceCollection.AddMemoryCache();
ioc.ServiceCollection.AddSingleton<ICacheService, MemoryCacheService>();

using (IServiceScope scope = ioc.CriarEscopo())
{
    var service = scope.ServiceProvider.GetService<IIntegracaoAppService>();

    service.IntegrarUsuarios();

}

