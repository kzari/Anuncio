using Lopes.Acesso.ConsoleCommons.Log;
using Lopes.Acesso.MemoryCache;
using Lopes.Botmaker.Application.Services;
using Lopes.Botmaker.IoC;
using Lopes.Domain.Common.IoC;
using Lopes.Domain.Commons.Cache;
using Lopes.Infra.Common;
using Microsoft.Extensions.DependencyInjection;

IoC ioc = IoC.ConfigurarServicos<ConsoleLogger>(new BotmakerIoC(TipoBaseDados.Producao));

ioc.ServiceCollection.AddMemoryCache();
ioc.ServiceCollection.AddSingleton<ICacheService, MemoryCacheService>();

using (IServiceScope scope = ioc.CriarEscopo())
{
    var service = scope.ServiceProvider.GetService<IIntegracaoAppService>();

    service.IntegrarTudo();

}

