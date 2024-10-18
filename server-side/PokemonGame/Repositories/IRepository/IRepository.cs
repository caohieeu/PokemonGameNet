using MongoDB.Driver;

namespace PokemonGame.Repositories.IRepository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class 
    {
        Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filter);
        Task<TEntity> FindById(string id);
        Task<bool> Add(TEntity entity);
    }
}
