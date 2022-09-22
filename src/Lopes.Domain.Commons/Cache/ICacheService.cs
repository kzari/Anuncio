namespace Lopes.Domain.Commons.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Grava em cache o dado para a chave
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chave">Chave</param>
        /// <param name="dado">Dado a ser guardado</param>
        /// <param name="expiracaoAPartirDeAgora">Tempo de expiração a partir de agora</param>
        /// <param name="tempoExpiracaoInatividade">Tempo de expiração por inatividade</param>
        /// <returns></returns>
        bool Gravar<T>(string chave, T dado, TimeSpan expiracaoAPartirDeAgora, TimeSpan? tempoExpiracaoInatividade = null);

        /// <summary>
        /// Retorna o dado gravado em cache (null se não encontrado)
        /// </summary>
        /// <typeparam name="T">Tipo do dado</typeparam>
        /// <param name="chave">Chave</param>
        /// <returns></returns>
        T? Obter<T>(string chave) where T : class;

        /// <summary>
        /// Tenta retornar o dado em cache e faz a gravação caso não seja encontrado
        /// </summary>
        /// <typeparam name="T">Tipo do dado</typeparam>
        /// <param name="chave">Chave</param>
        /// <param name="expiracaoAPartirDeAgora">Tempo de expiração a partir de agora</param>
        /// <param name="acao">Ação para resgatar o dado caso o mesmo não esteja no cache</param>
        /// <param name="tempoExpiracaoInatividade">Tempo de expiração por inatividade</param>
        /// <returns></returns>
        T? ObterOuGravar<T>(string chave, TimeSpan expiracaoAPartirDeAgora, Func<T> acao, TimeSpan? tempoExpiracaoInatividade = null) where T : class;

        /// <summary>
        /// Remove o dado do cache
        /// </summary>
        /// <param name="chave">Chave</param>
        void Remover(string chave);
    }
}