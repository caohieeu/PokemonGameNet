using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PokemonGame.Core.Models.SubModel
{
    public class Participant
    {
        [BsonElement("userId"), BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonElement("username"), BsonRepresentation(BsonType.String)]
        public string UserName { get; set; }
        [BsonElement("avatar"), BsonRepresentation(BsonType.String)]
        public string Avatar { get; set; }
    }
}
