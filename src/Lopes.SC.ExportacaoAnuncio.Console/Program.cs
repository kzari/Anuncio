﻿// See https://aka.ms/new-console-template for more information
using Lopes.SC.AnuncioXML.Domain.Models;
using Lopes.SC.ExportacaoAnuncio.Application.Interfaces;
using Lopes.SC.ExportacaoAnuncio.ConsoleTestes;
using Lopes.SC.ExportacaoAnuncio.Domain.Enums;
using Lopes.SC.Infra.IoC;
using Lopes.SC.Infra.XML;
using Lopes.SC.XML.Domain;
using Microsoft.Extensions.DependencyInjection;

//ServiceConfiguration.ConfigureServices(new ServiceCollection());

//var logger = new ConsoleLogger();
//var repo = new EmpresaApelidoPortalRepository(new DbLopesnetContext());
//var anuncioRepo = new AnuncioRepository(new DbProdutoContext());
//var anuncioApp = new AnuncioAppService(anuncioRepo);
//var repoAtuali = new ImovelAtualizacaoPortaisRepository(new DbProdutoContext());
//var imovelXmlService = new ImovelXMLAppService(@"C:\Temp\portais", repo, logger);
//var imovelRepo = new ImovelRepository(new DbProdutoContext());

//var retorno = imovelXmlService.ObterImoveisXMLs(@"C:\Temp\portais");

//var atualizarImovelAppService = new AtualizarImovelAppService(imovelXmlService, 
//                                                              logger,
//                                                              anuncioApp, 
//                                                              repoAtuali, 
//                                                              new DadosImovelAppService(imovelRepo),
//                                                              new StatusAnuncioService(imovelRepo));
//atualizarImovelAppService.AtualizarPorImoveis(new int[] { 627841 } );


//IServiceProvider provider = ServiceConfiguration.ConfigureServices<ConsoleLogger>(new ServiceCollection());
//using (IServiceScope scope = provider.CreateScope())
//{
//    IAtualizarAnunciosAppService atualizarImovelAppService = scope.ServiceProvider.GetService<IAtualizarAnunciosAppService>();
//    atualizarImovelAppService.AtualizarPorPortais(new Portal[] { Portal.Zap });
//}


var zap = new Zap();
var elementoCabecalho = zap.CriarElementoCabecalho();
var elementoImovel = zap.CriarElementoImovel(new Dados 
{ 
    IdImovel = 123,

});

var builder = new PortalXMLBuilder("c:/temp/portais/zap-teste.xml");
builder.InserirAtualizarImovel(123, elementoCabecalho, elementoImovel);