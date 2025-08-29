using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Models.Dtos.RoomChat
{
    public class RemoveParticipantDto
    {
        public string RoomChatID { get; set; }
        public string UserId { get; set; }
    }
}
