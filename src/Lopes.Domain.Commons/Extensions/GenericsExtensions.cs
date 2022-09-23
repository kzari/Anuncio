namespace Lopes.Infra.Commons.Extensions
{
    public static class GenericsExtensions
    {
        /// <summary>
        /// Verifica se lista possui algum item 'de forma segura'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool Algum<T>(this IEnumerable<T>? lista) => lista?.Any() == true;
        /// <summary>
        /// Verifica se lista está vazia ou nula
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool Nenhum<T>(this IEnumerable<T> lista) => !Algum(lista);
    }
}