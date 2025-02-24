using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Dtos.RoomChat;
using PokemonGame.Models;

namespace PokemonGame.Services.IService
{
    public interface IRoomBattleService
    {
        Task AddUserToConnection(string username, string connectionId);
        Task RemoveUserConnection(string username, string connectionId);
        string GetUserFromConnection(string connectionId);
        List<string> GetUserConnections(string username);
        Task<RoomBattle> AddRoomBattle();
        Task<bool> UpdateRoomBattle(RoomBattle roomBattle);
        Task<bool> RemoveRoomBattle(string roomId);
        Task<bool> AddParticipant(RandomPokemonDto randomPokemonDto);
        Task<bool> UpdateStatusParticipant(string roomId, string username, string status);
        Task<bool> RemoveUserFromRoom(string roomId, string username);
        Task<RoomBattle> GetRoomBattle(string roomId);
        Task<ParticipantRoomBattleDto> GetParticipant(string roomId, string username);
        Task<ParticipantRoomBattleDto> GetUserFromRoomBattle(string roomId, string username);
        Task<bool> UpdateStatusRoomBattle(string roomId, string status);
        Task<PokemonTeamDto> GetPokemonRoomBattle(string roomId, int pokemonId);
        Task<bool> SwitchPokemon(SwitchPokemonDto switchPokemon);
        Task<bool> SwitchRemainingPokemon(string roomId, string playerId, List<PokemonTeamDto> pokemons);
        Task ExcuteWinner(string roomId, string userName);
        Task<bool> UpdateCurrentTurn(string roomId, string username);
        Task<BattleResultDto> ApplyMove(PokemonTeamDto attacker,  PokemonTeamDto defender, MoveStateDto move);
    }
}
