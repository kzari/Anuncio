using Lopes.Botmaker.Application.Services;
using Lopes.Botmaker.IoC;
using Microsoft.Extensions.DependencyInjection;

BotmakerIoC ioc = new BotmakerIoC().Build();

using (IServiceScope scope = ioc.CriarEscopo())
{
    var service = scope.ServiceProvider.GetService<IIntegracaoAppService>();

    service.IntegrarTudo();

}

