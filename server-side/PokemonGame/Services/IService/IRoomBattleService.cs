using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Models;

namespace PokemonGame.Services.IService
{
    public interface IRoomBattleService
    {
        Task AddUserToConnection(string username, string connectionId);
        Task RemoveUserConnection(string username, string connectionId);
        List<string> GetUserConnections(string username);
        Task<RoomBattle> AddRoomBattle();
        Task<bool> RemoveRoomBattle(string roomId);
        Task<bool> AddParticipant(RandomPokemonDto randomPokemonDto);
        Task<RoomBattle> GetRoomBattle(string roomId);
        Task<Models.Pokemon> GetPokemonRoomBattle(string roomId, int pokemonId);
        Task<bool> SwitchPokemon(SwitchPokemonDto switchPokemon);
    }
}
