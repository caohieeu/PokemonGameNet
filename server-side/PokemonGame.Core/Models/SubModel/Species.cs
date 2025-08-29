using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Core.Models.SubModel
{
    public class Species
    {
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        [BsonElement("url"), BsonRepresentation(BsonType.String)]
        public string Url { get; set; }
    }
}
