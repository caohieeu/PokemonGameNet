using MongoDB.Driver;

namespace PokemonGame.DAL
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; set; }
        IMongoClient Client { get; set; }
    }
}
