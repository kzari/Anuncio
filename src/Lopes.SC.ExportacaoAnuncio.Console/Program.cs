// See https://aka.ms/new-console-template for more information
using Konsole;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.ConsoleTestes;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.Infra.IoC;
using Microsoft.Extensions.DependencyInjection;


//var progresso = new ProgressBar(10);
//progresso.Refresh(1, "A");
//progresso.Refresh(2, "B");
//progresso.Refresh(3, "C");
//progresso.Refresh(4, "C");
//progresso.Refresh(5, "C");
//progresso.Refresh(6, "C");
//progresso.Next("DD");
//progresso.Refresh(8, "C");


IServiceProvider provider = ServiceConfiguration.ConfigureServices<ConsoleLogger>(new ServiceCollection());
using (IServiceScope scope = provider.CreateScope())
{
    IAtualizarAnunciosAppService atualizarImovelAppService = scope.ServiceProvider.GetService<IAtualizarAnunciosAppService>();
    atualizarImovelAppService.AtualizarPorPortais(new Portal[] { Portal.Zap });

    //atualizarImovelAppService.AtualizarPorCotas(new [] { 48 });
}