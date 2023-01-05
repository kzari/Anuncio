using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lopes.Domain.Common.IoC
{
    public interface IBaseIoC
    {
        void ConfigurarServicos(IServiceCollection services, IConfiguration configuration);
    }
}