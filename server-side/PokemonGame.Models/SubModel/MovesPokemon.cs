using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Models.SubModel
{
    public class MovesPokemon
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.Int64)]
        public long Id { get; set; }
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        [BsonElement("type")]
        public Species Type { get; set; }
        [BsonElement("power"), BsonRepresentation(BsonType.Int64)]
        public long Power { get; set; }
        [BsonElement("priority"), BsonRepresentation(BsonType.Int64)]
        public long Priority { get; set; }
        [BsonElement("pp"), BsonRepresentation(BsonType.Int64)]
        public long PP { get; set; }
        [BsonElement("accuracy"), BsonRepresentation(BsonType.Int64)]
        public long Accuracy { get; set; }
    }
}
