using Lopes.Domain.Common.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Infra.Commons
{
    public class ConfiguracaoIoCBase
    {
        public ConfiguracaoIoCBase(TipoBaseDados tipoBaseDados)
        {
            TipoBaseDados = tipoBaseDados;
        }

        public TipoBaseDados TipoBaseDados { get; protected set; }


        public void RegistrarContextoEF<TDbContext>(IServiceCollection services, IConfiguration configuration, string chave) where TDbContext : DbContext
        {
            string stringConexao = ObterStringConexao(chave, configuration);
            
            services.AddDbContext<TDbContext>(_ => _.UseSqlServer(stringConexao), ServiceLifetime.Transient);
        }

        private string ObterStringConexao(string chave, IConfiguration configuration)
        {
            string complementoChave = ";";

            switch (TipoBaseDados)
            {
                case TipoBaseDados.Producao:
                    complementoChave = "PRD";
                    break;
                case TipoBaseDados.Hml:
                    complementoChave = "HML";
                    break;
                case TipoBaseDados.D_1:
                    complementoChave = "D_1";
                    break;
            }

            chave = $"{chave}_{complementoChave}";
            string stringConexao  = configuration.GetConnectionString(chave);

            if (string.IsNullOrEmpty(stringConexao))
                throw new Exception($"String de conexão com a chave '{chave}' não encontrada.");
            
            return stringConexao;
        }
    }
}