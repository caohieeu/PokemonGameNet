using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Dtos.RoomChat
{
    public class JoinRoomDto
    {
        public string RoomChatID { get; set; }
        public Participant Participant { get; set; }
    }
}
