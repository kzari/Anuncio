namespace Julio.Domain.Commons
{
    public static class ServiceProviderExtensions
    {
        public static T ObterServico<T>(this IServiceProvider provider)
        {
            T? servico = (T)provider.GetService(typeof(T));
            if (servico == null)
                throw new Exception($"Serviço tipo {typeof(T).Name} não encontrado");

            return servico;
        }
    }
}
