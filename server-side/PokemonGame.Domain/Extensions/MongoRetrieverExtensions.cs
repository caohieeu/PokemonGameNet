using MongoDB.Driver;
using PokemonGame.Domain.Helpers;

namespace PokemonGame.Domain.Extensions
{
    public static class MongoRetrieverExtensions
    {
        public static async Task<List<T>> RetrieveAsync<T>(
            this IMongoCollection<T> collection,
            Func<IFindFluent<T, T>, IFindFluent<T, T>> queryBuilder,
            Action<MongoCacheBuilder<T>> cacheBuilder = null
        )
        {
            var query = collection.Find(Builders<T>.Filter.Empty);
            var builtQuery = queryBuilder(query);

            var result = await builtQuery.ToListAsync();

            if(cacheBuilder != null)
            {
                var cacheConfig = new MongoCacheBuilder<T>(result);
                cacheBuilder(cacheConfig);
            }
            return null;
        }
    }
}
