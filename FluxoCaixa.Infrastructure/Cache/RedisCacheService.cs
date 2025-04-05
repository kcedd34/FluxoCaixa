using FluxoCaixa.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<RedisCacheService> _logger;
        private bool _redisAvailable = true;

        public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<T> ObterAsync<T>(string chave) where T : class
        {
            if (!_redisAvailable)
                return null;

            try
            {
                var cachedValue = await _distributedCache.GetStringAsync(chave);

                if (string.IsNullOrEmpty(cachedValue))
                    return null;

                return JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (Exception ex)
            {
                _redisAvailable = false;
                _logger.LogWarning(ex, "Redis não está disponível. Cache será desativado temporariamente.");
                return null;
            }
        }

        public async Task ArmazenarAsync<T>(string chave, T valor, TimeSpan? expiracao = null) where T : class
        {
            if (!_redisAvailable)
                return;

            try
            {
                var options = new DistributedCacheEntryOptions();

                if (expiracao.HasValue)
                    options.AbsoluteExpirationRelativeToNow = expiracao.Value;
                else
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                var serializedValue = JsonSerializer.Serialize(valor);
                await _distributedCache.SetStringAsync(chave, serializedValue, options);
            }
            catch (Exception ex)
            {
                _redisAvailable = false;
                _logger.LogWarning(ex, "Redis não está disponível. Cache será desativado temporariamente.");
            }
        }

        public async Task RemoverAsync(string chave)
        {
            if (!_redisAvailable)
                return;

            try
            {
                await _distributedCache.RemoveAsync(chave);
            }
            catch (Exception ex)
            {
                _redisAvailable = false;
                _logger.LogWarning(ex, "Redis não está disponível. Cache será desativado temporariamente.");
            }
        }
    }
}