using FluxoCaixa.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T> ObterAsync<T>(string chave) where T : class
        {
            if (_memoryCache.TryGetValue(chave, out T value))
                return Task.FromResult(value);

            return Task.FromResult<T>(null);
        }

        public Task ArmazenarAsync<T>(string chave, T valor, TimeSpan? expiracao = null) where T : class
        {
            var cacheOptions = new MemoryCacheEntryOptions();

            if (expiracao.HasValue)
                cacheOptions.AbsoluteExpirationRelativeToNow = expiracao.Value;
            else
                cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            _memoryCache.Set(chave, valor, cacheOptions);

            return Task.CompletedTask;
        }

        public Task RemoverAsync(string chave)
        {
            _memoryCache.Remove(chave);

            return Task.CompletedTask;
        }
    }
}