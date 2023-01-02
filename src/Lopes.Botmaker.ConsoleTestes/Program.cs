using Lopes.Acesso.ConsoleCommons.Log;
using Lopes.Botmaker.Application.Services;
using Lopes.Botmaker.IoC;
using Microsoft.Extensions.DependencyInjection;

BotmakerIoC ioc = new BotmakerIoC().Build<ConsoleLogger>();

using (IServiceScope scope = ioc.CriarEscopo())
{
    var service = scope.ServiceProvider.GetService<IIntegracaoAppService>();

    service.IntegrarTudo();

}

