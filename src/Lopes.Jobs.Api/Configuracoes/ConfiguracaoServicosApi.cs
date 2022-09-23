using Hangfire;
using Hangfire.Console;
using Lopes.Anuncio.IoC;
using Lopes.Domain.Commons.Cache;
using Lopes.Infra.Cache;
using Lopes.Jobs.Api.Log;

namespace Lopes.Jobs.Api.Configuracoes
{
    public class ConfiguracaoServicosApi : ConfiguracaoServicos
    {
        public static WebApplicationBuilder Configurar(WebApplicationBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .Build();

            //Hangfire
            builder.Services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(configuration.GetConnectionString("DbLopesnet"));
                x.UseConsole();
            });
            builder.Services.AddHangfireServer();

            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ConfigurarServicos<HangFireLog>(configuration, builder.Services);

            return builder;
        }


        protected override void RegistrarCache(IServiceCollection services, IConfiguration configuration)
        {
            string configuracaoRedis = configuration["Redis.Conexao"];
            if (!string.IsNullOrEmpty(configuracaoRedis))
            {
                services.AddStackExchangeRedisCache(options => options.Configuration = configuracaoRedis);
                services.AddSingleton<ICacheService, CacheDistribuidoService>();
            }
            else
            {
                Console.WriteLine("A configuração do servidor do Redis está vazia, usando cache em memória.");
                base.RegistrarCache(services, configuration);
            }
        }
    }
}
