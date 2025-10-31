using MongoDB.Driver;

namespace PokemonGame.Persistence.DAL
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; set; }
        IMongoClient Client { get; set; }
    }
}
