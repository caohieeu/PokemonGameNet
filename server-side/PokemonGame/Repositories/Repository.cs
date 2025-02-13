using MongoDB.Driver;
using PokemonGame.DAL;
using PokemonGame.Exceptions;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Settings;

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
        public async Task<bool> Add(TEntity entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public async Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }
        protected static FilterDefinition<TEntity> FilterId(string key, string keyValue)
        {
            return Builders<TEntity>.Filter.Eq(key, keyValue);
        }

        public async Task<IEnumerable<TEntity>> GetMany(int page, int size)
        {
            var all = _collection.Find(Builders<TEntity>.Filter.Empty)
                .Skip(size * (page - 1))
                .Limit(size);
            return await all.ToListAsync();
        }

        public async Task<PaginationModel<TEntity>> GetManyByFilter(int page, int pageSize, FilterDefinition<TEntity> filter, SortDefinition<TEntity> sorDef)
        {
            var data = await _collection.Find(filter)
                .Sort(sorDef)
                .Skip(pageSize * (page - 1))
                .Limit(pageSize)
                .ToListAsync();

            var pagination = new PaginationModel<TEntity>();
            if(data.Count == 0)
            {
                pagination.data = null;
                pagination.page = page;
                pagination.pageSize = pageSize;
                pagination.total_pages = 0;
                pagination.total_rows = 0;
                return pagination;
            }
            pagination.data = data.ToList();
            pagination.page = page;
            pagination.pageSize = data.Count;
            pagination.total_rows = (int)(await _collection.Find(filter).CountDocumentsAsync());
            pagination.total_pages = (int)Math.Ceiling(pagination.total_rows / (double)pageSize);

            return pagination;
        }
        public async Task<bool> IsExist(FilterDefinition<TEntity>? filter)
        {
            return await _collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task<TEntity> GetByFilter(FilterDefinition<TEntity> filter)
        {
            try
            {
                var data = await _collection.FindAsync(filter);
                var res = data.SingleOrDefault();
                return res;
            } catch
            {
                throw new NotFoundException("This content is not found");
            }
        }

        public async Task<bool> UpdateOneByFilter(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            var res = await _collection.UpdateOneAsync(filter, update);
            return res.ModifiedCount > 0;
        }

        public async Task<bool> Remove(string id)
        {
            try
            {
                var filter = FilterId("Id", id);

                var res = await _collection.DeleteOneAsync(filter);

                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> ReplaceOneAsync(FilterDefinition<TEntity> filter, TEntity entity)
        {
            var result = await _collection.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }
    }
}
