using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using PokemonGame.Core.Constants;
using PokemonGame.Domain.Exceptions;
using PokemonGame.Core.Interfaces.Repositories;
using PokemonGame.Persistence.DAL;
using PokemonGame.Core.Settings;
using PokemonGame.Core.Interfaces.Services;
using System.Runtime.CompilerServices;

namespace PokemonGame.Domain.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoDatabase _database;
        protected readonly IMongoClient _client;
        protected readonly IMongoCollection<TEntity> _collection;
        protected readonly IMongoContext _context;
        protected readonly ICacheService _cacheService;

        public Repository(IMongoContext context, ICacheService cacheService)
        {
            _database = context.Database;
            _client = context.Client;
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
            _context = context;
            _cacheService = cacheService;
        }

        //public Repository(IMongoContext context)
        //{
        //    this._context = context;
        //}

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

        public async Task<PaginationModel<TEntity>> GetManyByFilter(int page, int pageSize, FilterDefinition<TEntity> filter, string cacheKey)
        {
            var sortDef = Builders<TEntity>.Sort.Ascending("Id");

            using var cursorData = await _collection.Find(filter)
                .Sort(sortDef)
                .Skip(pageSize * (page - 1))
                .Limit(pageSize)
                .ToCursorAsync();

            var pagination = new PaginationModel<TEntity>()
            {
                page = page,
                pageSize = pageSize,
                total_rows = (int)(await _collection.Find(filter).CountDocumentsAsync())
            };
            pagination.total_pages = (int)Math.Ceiling(pagination.total_rows / (double)pageSize);

            var results = await _cacheService.GetCacheAsync<List<TEntity>>(cacheKey);

            if (results == null)
            {
                results = new List<TEntity>();
                while (await cursorData.MoveNextAsync())
                {
                    results.AddRange(cursorData.Current);
                }
            }

            pagination.data = results;

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
                throw new NotFoundException(ExceptionMessage.CONTENT_NOT_FOUND);
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
        public async Task<IEnumerable<TResult>> GetFieldValueAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector, 
            FilterDefinition<TEntity>? filter = null)
        {
            var effectiveFilter = filter ?? Builders<TEntity>.Filter.Empty;
            return await _collection
                .Find(effectiveFilter)
                .Project(fieldSelector)
                .ToListAsync();

        }
    }
}
