namespace Lopes.Infra.Commons.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Verifica se array possui algum item 'de forma segura'
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool Algum<T>(this T[]? array) => array?.Length > 0;
        /// <summary>
        /// Verifica se array está vazio ou nulo
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool Nenhum<T>(this T[]? array) => !Algum(array);
    }
}