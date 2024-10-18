using MongoDB.Driver;
using PokemonGame.DAL;
using PokemonGame.Repositories.IRepository;
using ZstdSharp.Unsafe;

namespace PokemonGame.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoDatabase _database;
        protected readonly IMongoClient _client;
        protected readonly IMongoCollection<TEntity> _collection;
        public Repository(IMongoContext context)
        {
            _database = context.Database;
            _client = context.Client;
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        public Task<bool> Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<TEntity> GetByFilter(FilterDefinition<TEntity> filter)
        {
            var data = await _collection.FindAsync(filter);
            return data.SingleOrDefault();
        }

        public async Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
