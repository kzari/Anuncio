// See https://aka.ms/new-console-template for more information
using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Domain.Enums;
using Lopes.Infra.ConsoleCommons.Log;
using Lopes.Infra.IoC;
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
    IAtualizarAnunciosAppService atualizarAnuncioService = scope.ServiceProvider.GetService<IAtualizarAnunciosAppService>();
    // atualizarAnuncioService.AtualizarPorPortais(new Portal[] { Portal.Zap }, null);

    //atualizarAnuncioService.AtualizarPorCotas(new [] { 48 }, null);

    atualizarAnuncioService.AtualizarPorImoveis(new[] { 569521 }, null, null);
}