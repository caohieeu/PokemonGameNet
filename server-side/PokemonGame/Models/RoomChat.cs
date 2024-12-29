using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Models
{
    public class RoomChat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string Description { get; set; }
        [BsonElement("participants")]
        public List<Participant> Participants { get; set; }
        [BsonElement("dateCreated")]
        public DateTime DateCreated { get; set; }
    }
}
