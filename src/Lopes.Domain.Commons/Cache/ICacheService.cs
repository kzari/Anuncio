namespace Lopes.Domain.Commons.Cache
{
    public interface ICacheService
    {
        bool Gravar<T>(string chave, T dado, TimeSpan expiracaoAPartirDeAgora);
        bool Gravar<T>(string chave, T dado, DateTimeOffset expiracao);
        T? Obter<T>(string chave) where T : class;
        T? ObterOuGravar<T>(string chave, DateTimeOffset expiracao, Func<T> acao) where T : class;
        T? ObterOuGravar<T>(string chave, TimeSpan expiracaoAPartirDeAgora, Func<T> acao) where T : class;
        void Remover(string chave);
    }
}