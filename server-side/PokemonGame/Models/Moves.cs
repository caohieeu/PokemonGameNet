using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Models
{
    public class Moves
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public long Id { get; set; }
        public string Name { get; set; }
        // Nếu bạn muốn lưu danh sách các ID của loại
        // public List<long> TypeIds { get; set; } 

        public Species Type { get; set; }
        public long Power { get; set; }
        public long Priority { get; set; }
        public long EffectChance { get; set; }
        public long PP { get; set; }
        public long Accuracy { get; set; }
    }
}
