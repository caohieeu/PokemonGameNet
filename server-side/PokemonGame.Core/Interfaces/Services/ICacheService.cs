using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Distributed;
using PokemonGame.Core.Helpers;

namespace PokemonGame.Core.Interfaces.Services
{
    public interface ICacheService
    {
        Task SetCache(string cacheKey, object data, DistributedCacheEntryOptions options);
        Task RemoveCache(string cacheKey);
        Task<T> GetCacheAsync<T>(string cacheKey);
        //T RetrieverData(Func<MongoCacheBuilder<T>, T> cache, string cacheKey);
    }
}
