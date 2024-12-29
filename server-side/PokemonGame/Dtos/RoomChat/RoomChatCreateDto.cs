using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Dtos.RoomChat
{
    public class RoomChatCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
