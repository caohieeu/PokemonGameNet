using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Models.SubModel
{
    public class Abilities
    {
        [BsonElement("ability")]
        public Species Ability { get; set; }
        [BsonElement("is_hidden")]
        public bool IsHidden { get; set; }
        [BsonElement("slot")]
        public int Slot { get; set; }
    }
}
