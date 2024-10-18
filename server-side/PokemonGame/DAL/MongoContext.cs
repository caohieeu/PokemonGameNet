using MongoDB.Driver;
using PokemonGame.Settings;

namespace PokemonGame.DAL
{
    public class MongoContext : IMongoContext
    {
        public MongoContext(IDatabaseSetting databaseSetting)
        {
            var settings = MongoClientSettings.FromConnectionString(databaseSetting.DBConnection);

            Client = new MongoClient(settings);
            Database = Client.GetDatabase(databaseSetting.DatabaseName);
        }
        public IMongoDatabase Database { get; set; }
        public IMongoClient Client { get; set; }
    }
}
