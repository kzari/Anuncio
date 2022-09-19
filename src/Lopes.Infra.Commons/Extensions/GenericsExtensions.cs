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
        public static bool Algum<T>(this IEnumerable<T>? lista)
        {
            return lista != null && lista.Any();
        }

        public static bool Nenhum<T>(this IEnumerable<T> lista) => !Algum(lista);
    }
    public static class ArrayExtensions
    {
        /// <summary>
        /// Verifica se array possui algum item 'de forma segura'
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool Algum<T>(this T[]? array) where T : struct
        {
            return array != null && array.Length > 0;
        }
        /// <summary>
        /// Verifica se array está vazio ou nulo
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool Nenhum<T>(this T[]? array) where T : struct => !Algum(array);
    }
}
