using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Models.Dtos.RoomChat
{
    public class JoinRoomDto
    {
        public string RoomChatID { get; set; }
        public Participant Participant { get; set; }
    }
}
