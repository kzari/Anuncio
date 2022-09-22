using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Application.Services;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Lopes.Domain.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using Lopes.Anuncio.Domain.Handlers;
using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Domain.Commons.Cache;
using Lopes.Infra.Cache;
using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Dados.Leitura.DadosService;
using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Application.Interfaces.DadosService;
using Lopes.Anuncio.Repositorio.Context;
using Lopes.Anuncio.Repositorio.Repositorios;

namespace Lopes.Infra.IoC
{
    public class ConfiguracaoServicos
    {
        public static IServiceCollection ConfigurarServicos<TLogger>(IConfiguration configuration = null, IServiceCollection services = null) where TLogger : class, ILogger
        {
            return new ConfiguracaoServicos().Configurar<TLogger>(configuration, services);
        }

        public virtual IServiceCollection Configurar<TLogger>(IConfiguration configuration = null, IServiceCollection services = null) where TLogger : class, ILogger
        {
            services ??= new ServiceCollection();

            configuration = RegistrarIConfiguration(services, configuration);

            RegistrarCache(services);
            RegistrarLog<TLogger>(services);
            RegistrarDadosServices(services);
            RegistrarRepositorios(services);
            RegistrarAppServices(services);
            RegistrarDbContexts(services, configuration);
            RegistrarFabricas(services);
            RegistrarDomainServices(services);
            RegistrarHandlers(services);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }


        protected virtual void RegistrarCache(IServiceCollection services)
        {
            //services.AddMemoryCache();
            //services.AddSingleton<ICacheService, MemoryCacheService>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
            services.AddSingleton<ICacheService, CacheDistribuidoService>();
        }

        protected virtual IConfiguration RegistrarIConfiguration(IServiceCollection services, IConfiguration configuration = null)
        {
            configuration ??= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddSingleton<IConfiguration>(configuration);
            return configuration;
        }

        protected virtual void RegistrarFabricas(IServiceCollection services)
        {
            services.AddTransient<IPortalAtualizadorFactory, PortalAtualizadorFactory>();
        }

        protected virtual void RegistrarHandlers(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<AnunciosAtualizacaoCommand, bool>, AtualizacaoCommandHandler>();
            services.AddScoped<IRequestHandler<RegistroAtualizacaoCommand, AtualizarStatusAnuncioResponse>, RegistroAtualizacaoCommandHandler>();
            services.AddScoped<IRequestHandler<RegistroAtualizacoesCommand, bool>, RegistroAtualizacaoCommandHandler>();
        }

        protected virtual void RegistrarDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbLopesnetContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbLopesnet")), ServiceLifetime.Transient);
            services.AddDbContext<DbProdutoContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbProduto")), ServiceLifetime.Transient);

            services.AddDbContext<DbLopesnetLeituraContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbLopesnet")), ServiceLifetime.Transient);
            services.AddDbContext<DbProdutoLeituraContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbProduto")), ServiceLifetime.Transient);
        }

        protected virtual void RegistrarLog<TLogger>(IServiceCollection services)  where TLogger : class, ILogger
        {
            services.AddSingleton<ILogger, TLogger>();
        }

        protected virtual void RegistrarDomainServices(IServiceCollection services)
        {
            services.AddTransient<IStatusAnuncioService, StatusAnuncioService>();
        }

        protected virtual void RegistrarAppServices(IServiceCollection services)
        {
            services.AddTransient<IAtualizacaoAppService, AtualizacaoAppService>();
            services.AddTransient<IRegistrarAtualizacaoAnunciosAppService, RegistrarAtualizacaoAppService>();
        }

        protected virtual void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddTransient<IAnuncioStatusRepositorio, AnuncioStatusRepositorio>();
        }
        protected virtual void RegistrarDadosServices(IServiceCollection services)
        {
            services.AddTransient<IProdutoService, ProdutoDadosAppService>();
            services.AddTransient<IProdutoDadosService, ProdutoDadosService>();
            services.AddTransient<IAnuncioDadosAppService, AnuncioDadosService>();
            services.AddTransient<IPortalCaracteristicaDadosAppService, PortalCaracteristicasDadosService>();
            services.AddTransient<IFranquiaApelidoPortalDadosAppService, FranquiaApelidoPortalDadosService>();
        }
    }
}