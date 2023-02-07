using Julio.Anuncio.Dados.Leitura.Context;
using Julio.Anuncio.Dados.Leitura.DadosService;
using Julio.Botmaker.Application.DadosServices;
using Julio.Botmaker.Application.Services;
using Julio.Domain.Common.IoC;
using Julio.Infra.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Julio.Botmaker.IoC
{
    public class BotmakerIoC : ConfiguracaoIoCBase, IConfiguracaoIoC
    {
        public BotmakerIoC(TipoBaseDados tipoBaseDados) : base(tipoBaseDados)
        {
        }

        public void ConfigurarServicos(IServiceCollection services, IConfiguration configuration)
        {
            RegistrarContextoEF<UsuarioLeituraContext>(services, configuration, "DbLopesnet");
            RegistrarAppServices(services, configuration);

            //RegistrarRepositorios(services);
            //RegistrarFabricas(services);
            //RegistrarDomainServices(services);
            //RegistrarHandlers(services);
            //services.AddMediatR(Assembly.GetExecutingAssembly());
        }
        private void RegistrarAppServices(IServiceCollection services, IConfiguration configuration)
        {
            string api = configuration["Botmaker.Api"];
            string token = configuration["Botmaker.Token"];
            services.AddSingleton<IBotmakerApiService>(new BotmakerApiService(token, api));

            services.AddScoped<IIntegracaoAppService, IntegracaoAppService>();
            services.AddScoped<IIntegracaoBotmakerDadosService, IntegracaoBotmakerDadosService>();
        }
    }
}