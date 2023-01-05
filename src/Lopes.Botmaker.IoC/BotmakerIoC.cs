using Lopes.Anuncio.Dados.Leitura.Context;
using Lopes.Anuncio.Dados.Leitura.DadosService;
using Lopes.Botmaker.Application.DadosServices;
using Lopes.Botmaker.Application.Services;
using Lopes.Domain.Common.IoC;
using Lopes.Infra.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Botmaker.IoC
{
    public class BotmakerIoC : BaseIoC, IBaseIoC
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