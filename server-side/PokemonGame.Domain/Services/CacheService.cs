using Microsoft.Extensions.Caching.Distributed;
using PokemonGame.Core.Helpers;
using PokemonGame.Core.Interfaces.Services;

namespace PokemonGame.Domain.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        //public T RetrieverData(Func<MongoCacheBuilder<T>, T> cache, string cacheKey)
        //{
        //    return default;
        //}

        public async Task SetCache(string cacheKey, object data, DistributedCacheEntryOptions options)
        {
            await _cache.SetStringAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(data), options);
        }
        public async Task RemoveCache(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }
        public async Task<T> GetCacheAsync<T>(string cacheKey)
        {
            return await _cache.GetStringAsync(cacheKey) is string cachedData
                ? System.Text.Json.JsonSerializer.Deserialize<T>(cachedData) ?? default
                : default;
        }
    }
}
