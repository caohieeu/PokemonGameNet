using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Models.Dtos.RoomChat
{
    public class RoomChatCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
