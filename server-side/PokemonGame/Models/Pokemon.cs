using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Models
{
    public class Pokemon
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.Int64)]
        public int Id { get; set; }
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        [BsonElement("type")]
        public List<string> Type { get; set; }
        [BsonElement("sprites")]
        public Sprites Sprites { get; set; }
        [BsonElement("species")]
        public Species Species { get; set; }
        [BsonElement("stat")]
        public Stat Stat { get; set; }
        [BsonElement("abilities")]
        public List<Abilities> Abilities { get; set; }
        [BsonElement("height"), BsonRepresentation(BsonType.Int32)]
        public double Height { get; set; }
        [BsonElement("weight"), BsonRepresentation(BsonType.Int32)]
        public double Weight { get; set; }
        [BsonElement("moves")]
        public List<MovesPokemon> Moves { get; set; }
    }
}
