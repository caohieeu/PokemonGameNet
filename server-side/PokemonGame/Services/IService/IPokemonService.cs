using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Models;
using PokemonGame.Models.SubModel;
using PokemonGame.Settings;

namespace PokemonGame.Services.IService
{
    public interface IPokemonService
    {
        Task<PaginationModel<Pokemon>> GetPokemonAsync(int page, int pageSize, string namePokemon);
        Task<Pokemon> GetDetailPokemonAsync(int pokemonId);
        Task<List<PokemonTeamDto>> GetRandomPokemons();
    }
}
