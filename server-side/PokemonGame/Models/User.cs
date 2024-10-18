using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PokemonGame.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("displayName"), BsonRepresentation(BsonType.String)]
        public string DisplayName { get; set; }
        [BsonElement("username"), BsonRepresentation(BsonType.String)]
        public string Username { get; set; }
        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string password;
        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string Email { get; set; }
        [BsonElement("phone"), BsonRepresentation(BsonType.String)]
        public string Phone { get; set; }
        [BsonElement("role"), BsonRepresentation(BsonType.String)]
        public HashSet<string> Roles { get; set; }
        //public long Point { get; set; }
        public List<string> Moves { get; set; } = null;
        public List<Pokemon> Pokemons { get; set; } = null;
        [BsonElement("dateCreated"), BsonRepresentation(BsonType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
