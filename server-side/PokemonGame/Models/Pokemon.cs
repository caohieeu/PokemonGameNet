using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Models
{
    public class Pokemon
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public string Species { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Abilities { get; set; }
        public long Hp { get; set; }
        public long Atk { get; set; }
        public long Defense { get; set; }
        public long SpAtk { get; set; }
        public long SpDef { get; set; }
        public long Speed { get; set; }
        public long Total { get; set; }
        public Moves Moves { get; set; }
    }
}
