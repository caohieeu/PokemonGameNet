using PokemonGame.Core.Models.Dtos.RoomChat;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Interfaces.Services
{
    public interface IRoomChatService
    {
        Task<List<RoomChat>> GetAllRoomChat();
        Task<RoomChat> GetRoomChat(string roomId);
        Task<bool> AddNewRoom(RoomChatCreateDto roomChatRequestDto);
        Task<bool> AddUserToRoom(JoinRoomDto joinRoomDto);
        Task<bool> RemoveUserFromRoom(RemoveParticipantDto removeParticipantDto);
        Task AddUserToConnection(string username, string connectionId);
        Task RemoveUserConnection(string username, string connectionId);
        List<string> GetUserConnections(string username);
        Task<bool> IsExistParticipant(string roomId, string userId);
        Task<List<Participant>> GetParticipants(string roomId);
    }
}
