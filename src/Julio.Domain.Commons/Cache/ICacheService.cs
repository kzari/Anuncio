namespace Julio.Domain.Commons.Cache
{
    public interface ICacheService
    {
        bool Gravar<T>(string chave, T dado, TimeSpan expiracaoAPartirDeAgora);
        bool Gravar<T>(string chave, T dado, DateTimeOffset expiracao);
        T? Obter<T>(string chave) where T : class;

        TResult? ObterOuGravar<TResult>(string chave, DateTimeOffset expiracao, Func<TResult> acao) where TResult : class;
        TResult? ObterOuGravar<TResult>(string chave, TimeSpan expiracaoAPartirDeAgora, Func<TResult> acao) where TResult : class;

        void Remover(string chave);
    }
}