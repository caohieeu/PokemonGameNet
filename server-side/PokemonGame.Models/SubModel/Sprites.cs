using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Models.SubModel
{
    public class Sprites
    {
        [BsonElement("image"), BsonRepresentation(BsonType.String)]
        public string Image {  get; set; }
        [BsonElement("back"), BsonRepresentation(BsonType.String)]
        public string Back { get; set; }
        [BsonElement("front"), BsonRepresentation(BsonType.String)]
        public string Front { get; set; }
    }
}
