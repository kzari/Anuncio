using Microsoft.Extensions.Caching.Memory;
using Lopes.Domain.Commons.Cache;

namespace Lopes.Infra.MemoryCache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public bool Gravar<T>(string chave, T dado, TimeSpan expiracaoAPartirDeAgora)
        {
            _memoryCache.Set(chave, dado, expiracaoAPartirDeAgora);
            return true;
        }

        public bool Gravar<T>(string chave, T dado, DateTimeOffset expiracao)
        {
            _memoryCache.Set<T>(chave, dado, expiracao);
            return true;
        }

        public T? Obter<T>(string chave) where T : class
        {
            return _memoryCache.TryGetValue(chave, out T dado) ? dado : null;
        }

        public T? ObterOuGravar<T>(string chave, Func<T> acao, TimeSpan expiracaoAPartirDeAgora) where T : class
        {
            T dado = _memoryCache.GetOrCreate(chave, entry => {
                entry.AbsoluteExpirationRelativeToNow = expiracaoAPartirDeAgora;
                return acao();
            });

            return dado;
        }

        public T? ObterOuGravar<T>(string chave, Func<T> acao, DateTimeOffset expiracao) where T : class
        {
            T dado = _memoryCache.GetOrCreate(chave, entry => {
                entry.AbsoluteExpiration = expiracao;
                return acao();
            });
            return dado;
        }

        public void Remover(string chave)
        {
            _memoryCache.Remove(chave);
        }
    }
}