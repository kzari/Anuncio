using Lopes.Domain.Commons.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Lopes.Infra.Cache
{
    public class CacheDistribuidoService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheDistribuidoService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public bool Gravar<T>(string chave, T dado, TimeSpan tempoExpiracao, TimeSpan? tempoExpiracaoInatividade = null)
        {
            string dadoSerealizado = JsonSerializer.Serialize(dado);

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(tempoExpiracao);
            if (tempoExpiracaoInatividade.HasValue)
                options.SetSlidingExpiration(tempoExpiracaoInatividade.Value);

            _distributedCache.SetString(chave, dadoSerealizado, options);

            return true;
        }

        public T? Obter<T>(string chave) where T : class
        {
            string? dadoSerializado = _distributedCache.GetString(chave);
            if (dadoSerializado == null)
                return null;

            return JsonSerializer.Deserialize<T>(dadoSerializado);
        }

        public T? ObterOuGravar<T>(string chave, TimeSpan tempoExpiracao, Func<T> acao, TimeSpan? tempoExpiracaoInatividade = null) where T : class
        {
            T? dado = Obter<T>(chave);
            if (dado == null)
            {
                dado = acao();
                Gravar(chave, dado, tempoExpiracao, tempoExpiracaoInatividade);
            }
            return dado;
        }

        public void Remover(string chave)
        {
            _distributedCache.Remove(chave);
        }
    }
}