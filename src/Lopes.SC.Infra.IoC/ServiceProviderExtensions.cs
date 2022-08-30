namespace Lopes.SC.Infra.IoC
{
    public static class ServiceProviderExtensions
    {
        public static T? ObterServico<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
