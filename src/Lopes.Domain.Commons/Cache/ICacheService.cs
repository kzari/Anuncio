namespace Lopes.Domain.Commons.Cache
{
    public interface ICacheService
    {
        bool Gravar<T>(string chave, T dado, TimeSpan expiracaoAPartirDeAgora);
        bool Gravar<T>(string chave, T dado, DateTimeOffset expiracao);
        T? Obter<T>(string chave) where T : class;
        T? ObterOuGravar<T>(string chave, Func<T> acao, DateTimeOffset expiracao) where T : class;
        T? ObterOuGravar<T>(string chave, Func<T> acao, TimeSpan expiracaoAPartirDeAgora) where T : class;
        void Remover(string chave);
    }
}