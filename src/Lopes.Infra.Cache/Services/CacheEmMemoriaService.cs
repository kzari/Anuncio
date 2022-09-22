using Microsoft.Extensions.Caching.Memory;
using Lopes.Domain.Commons.Cache;

namespace Lopes.Infra.MemoryCache
{
    public class CacheEmMemoriaService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheEmMemoriaService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public bool Gravar<T>(string chave, T dado, TimeSpan expiracaoAPartirDeAgora, TimeSpan? tempoExpiracaoInatividade = null)
        {
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiracaoAPartirDeAgora);
            if (tempoExpiracaoInatividade.HasValue)
                options.SetSlidingExpiration(tempoExpiracaoInatividade.Value);

            _memoryCache.Set(chave, dado, options);

            return true;
        }

        public T? Obter<T>(string chave) where T : class
        {
            return _memoryCache.TryGetValue(chave, out T dado) ? dado : null;
        }

        public T? ObterOuGravar<T>(string chave, TimeSpan expiracaoAPartirDeAgora, Func<T> acao, TimeSpan? tempoExpiracaoInatividade = null) where T : class
        {
            T dado = _memoryCache.GetOrCreate(chave, entry => {
                entry.AbsoluteExpirationRelativeToNow = expiracaoAPartirDeAgora;
                entry.SlidingExpiration = tempoExpiracaoInatividade;
                return acao();
            });

            return dado;
        }

        public T? ObterOuGravar<T>(string chave, DateTimeOffset expiracao, Func<T> acao) where T : class
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