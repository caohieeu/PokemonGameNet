using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Models.Entities
{
    public class Moves
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.Int64)]
        public long Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("power")]
        public long Power { get; set; }
        [BsonElement("pp")]
        public long PP { get; set; }
        [BsonElement("accuracy")]
        public long Accuracy { get; set; }
        [BsonElement("effect")]
        public string Effect { get; set; }
        [BsonElement("short_effect")]
        public string ShortEffect { get; set; }
    }
}
