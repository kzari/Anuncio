using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Domain.Common.IoC
{
    /// <summary>
    /// Interface para configuração de injeção de dependência
    /// </summary>
    public interface IConfiguracaoIoC
    {
        void ConfigurarServicos(IServiceCollection services, IConfiguration configuration);
    }
}