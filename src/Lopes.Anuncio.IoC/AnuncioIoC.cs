using Lopes.Anuncio.Application.Interfaces;
using Lopes.Anuncio.Application.Services;
using Lopes.Anuncio.Domain.Reposities;
using Lopes.Anuncio.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediatR;
using System.Reflection;
using Lopes.Anuncio.Domain.Handlers;
using Lopes.Anuncio.Domain.Commands.Responses;
using Lopes.Anuncio.Domain.Commands.Requests;
using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Dados.Leitura.DadosService;
using Lopes.Anuncio.Application.DadosService;
using Lopes.Anuncio.Application.Interfaces.DadosService;
using Lopes.Anuncio.Repositorio.Context;
using Lopes.Anuncio.Repositorio.Repositorios;
using Lopes.Acesso.App.Services;
using Lopes.Acesso.Dados.DadosServices;
using Lopes.Acesso.Application;
using Lopes.Acesso.Dados;
using Lopes.Acesso.IoC;
using Lopes.Domain.Common.IoC;
using Lopes.Infra.Commons;

namespace Lopes.Anuncio.IoC
{
    /// <summary>
    /// Configurações para as dependências relacionadas ao domínio de Anúncios
    /// </summary>
    public class AnuncioIoC : ConfiguracaoIoCBase, IConfiguracaoIoC
    {
        public AnuncioIoC(TipoBaseDados tipoBaseDados) : base(tipoBaseDados)
        {
        }

        public void ConfigurarServicos(IServiceCollection services, IConfiguration configuration)
        {
            RegistrarDadosServices(services);
            RegistrarRepositorios(services);
            RegistrarAppServices(services);
            RegistrarDbContexts(services, configuration);
            RegistrarFabricas(services);
            RegistrarDomainServices(services);
            RegistrarHandlers(services);

            services.AddMediatR(Assembly.GetExecutingAssembly());
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
            RegistrarContextoEF<DbLopesnetContext>(services, configuration, "DbLopesnet");
            //services.AddDbContext<DbLopesnetContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbLopesnet")), ServiceLifetime.Transient);

            RegistrarContextoEF<DbProdutoContext>(services, configuration, "DbProduto");
            //services.AddDbContext<DbProdutoContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbProduto")), ServiceLifetime.Transient);

            RegistrarContextoEF<DbLopesnetLeituraContext>(services, configuration, "DbLopesnet");
            //services.AddDbContext<DbLopesnetLeituraContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbLopesnet")), ServiceLifetime.Transient);
            RegistrarContextoEF<DbProdutoLeituraContext>(services, configuration, "DbProduto");
            //services.AddDbContext<DbProdutoLeituraContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbProduto")), ServiceLifetime.Transient);

            RegistrarContextoEF<AcessoDadosContext>(services, configuration, "DbLopesnet");
            //services.AddDbContext<AcessoDadosContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbLopesnet")), ServiceLifetime.Transient);
        }

        protected virtual void RegistrarDomainServices(IServiceCollection services)
        {
            services.AddTransient<IStatusAnuncioService, StatusAnuncioService>();

            services.AddTransient<ITokenService, JwtTokenService>();
        }

        protected virtual void RegistrarAppServices(IServiceCollection services)
        {
            services.AddTransient<IAtualizacaoAppService, AtualizacaoAppService>();
            services.AddTransient<IRegistrarAtualizacaoAnunciosAppService, RegistrarAtualizacaoAppService>();
            services.AddTransient<ICotaAppService, CotaAppService>();

            services.AddTransient<IUsuarioAcessoAppService, UsuarioAcessoAppService>();
        }

        protected virtual void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddTransient<IAnuncioStatusRepositorio, AnuncioStatusRepositorio>();
        }
        protected virtual void RegistrarDadosServices(IServiceCollection services)
        {
            services.AddTransient<IProdutoService, ProdutoDadosAppService>();
            services.AddTransient<IProdutoDadosService, ProdutoDadosService>();
            services.AddTransient<IAnuncioDadosService, AnuncioDadosService>();
            services.AddTransient<IPortalCaracteristicaDadosService, PortalCaracteristicasDadosService>();
            services.AddTransient<IFranquiaApelidoPortalDadosService, FranquiaApelidoPortalDadosService>();
            services.AddTransient<ICotaDadosService, CotaDadosService>();

            services.AddTransient<IUsuarioDadosService, UsuarioDadosService>();
        }
    }
}