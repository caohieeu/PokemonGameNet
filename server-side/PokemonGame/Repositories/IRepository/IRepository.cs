using System.Linq.Expressions;
using MongoDB.Driver;
using PokemonGame.Settings;

namespace PokemonGame.Repositories.IRepository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class 
    {
        Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filter);
        Task<IEnumerable<TEntity>> GetMany(int page, int size);
        Task<PaginationModel<TEntity>> GetManyByFilter(int page, int pageSize, FilterDefinition<TEntity> filter, SortDefinition<TEntity> sorDef);
        Task<bool> IsExist(FilterDefinition<TEntity>? filter);
        Task<TEntity> GetByFilter(FilterDefinition<TEntity> filter);
        //Task<TEntity> FindById(string id);
        Task<bool> Add(TEntity entity);
        Task<bool> Remove(string id);
        Task<bool> UpdateOneByFilter(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);
        Task<bool> ReplaceOneAsync(FilterDefinition<TEntity> filter, TEntity entity);
        Task<IEnumerable<TResult>> GetFieldValueAsync<TResult>(Expression<Func<TEntity, TResult>> fieldSelector,
            FilterDefinition<TEntity>? filter = null);
    }
}
