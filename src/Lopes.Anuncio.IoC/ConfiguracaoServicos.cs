using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Application.Services;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Lopes.Infra.Data.Context;
using Lopes.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Lopes.Domain.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using Lopes.Infra.Data.RepositoriosGravacao;
using Lopes.Anuncio.Domain.Handlers;
using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.Commands.Requests;

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

            services.AddMemoryCache();

            RegistrarLog<TLogger>(services);
            RegistrarRepositorios(services);
            RegistrarAppServices(services);
            RegistrarDbContexts(services, configuration);
            RegistrarFabricas(services);
            RegistrarDomainServices(services);
            RegistrarHandlers(services);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

        public virtual IConfiguration RegistrarIConfiguration(IServiceCollection services, IConfiguration configuration = null)
        {
            configuration ??= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddSingleton<IConfiguration>(configuration);
            return configuration;
        }

        public virtual void RegistrarFabricas(IServiceCollection services)
        {
            services.AddTransient<IPortalAtualizadorFactory, PortalAtualizadorFactory>();
        }

        public virtual void RegistrarHandlers(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<AnunciosAtualizacaoCommand, bool>, AnuncioAtualizadorHandler>();
            services.AddScoped<IRequestHandler<RegistroAtualizacaoCommand, AtualizarStatusAnuncioResponse>, RegistroAtualizacaoHandler>();
            services.AddScoped<IRequestHandler<RegistroAtualizacoesCommand, bool>, RegistroAtualizacaoHandler>();
        }

        public virtual void RegistrarDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbLopesnetContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbLopesnet")));
            services.AddDbContext<DbProdutoContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbProduto")));
        }

        public virtual void RegistrarLog<TLogger>(IServiceCollection services)  where TLogger : class, ILogger
        {
            services.AddSingleton<ILogger, TLogger>();
        }

        public virtual void RegistrarDomainServices(IServiceCollection services)
        {
            services.AddTransient<IStatusAnuncioService, StatusAnuncioService>();
        }

        public virtual void RegistrarAppServices(IServiceCollection services)
        {
            services.AddTransient<IAnuncioAppService, AnuncioAppService>();
            services.AddTransient<IAtualizacaoAppService, AtualizacaoAppService>();
            services.AddTransient<IDadosImovelAppService, DadosImovelAppService>();
            services.AddTransient<IRegistrarAtualizacaoAnunciosAppService, RegistrarAtualizacaoAnunciosAppService>();
        }
        public virtual void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddTransient<IEmpresaApelidoPortalRepository, EmpresaApelidoPortalRepository>();
            services.AddTransient<IImovelRepository, ImovelRepository>();
            services.AddTransient<IAnuncioRepository, AnuncioRepository>();
            services.AddTransient<IAnuncioStatusRepositorioGravacao, AnuncioStatusRepositorioGravacao>();
            services.AddTransient<IPortalCaracteristicaRepository, PortalCaracteristicaRepository>();
        }
    }
}