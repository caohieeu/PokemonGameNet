using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace PokemonGame.Models
{
    [BsonIgnoreExtraElements]
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<ObjectId>
    {
        [BsonElement("displayName"), BsonRepresentation(BsonType.String)]
        public string DisplayName { get; set; }
        [BsonElement("imagePath"), BsonRepresentation(BsonType.String)]
        public string ImagePath { get; set; }
        [BsonElement("role"), BsonRepresentation(BsonType.String)]
        public HashSet<string> Roles { get; set; }
        //public long Point { get; set; }
        public List<Pokemon> Pokemons { get; set; } = null;
        [BsonElement("dateCreated"), BsonRepresentation(BsonType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
