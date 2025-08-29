using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Dtos.RoomChat
{
    public class RemoveParticipantDto
    {
        public string RoomChatID { get; set; }
        public string UserId { get; set; }
    }
}
