using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PokemonGame.Models.SubModel
{
    public class Stat
    {
        [BsonElement("hp"), BsonRepresentation(BsonType.Int64)]
        public long Hp { get; set; }
        [BsonElement("atk"), BsonRepresentation(BsonType.Int64)]
        public long Atk { get; set; }
        [BsonElement("defense"), BsonRepresentation(BsonType.Int64)]
        public long Defense { get; set; }
        [BsonElement("sp_atk"), BsonRepresentation(BsonType.Int64)]
        public long SpAtk { get; set; }
        [BsonElement("sp_def"), BsonRepresentation(BsonType.Int64)]
        public long SpDef { get; set; }
        [BsonElement("speed"), BsonRepresentation(BsonType.Int64)]
        public long Speed { get; set; }
    }
}
