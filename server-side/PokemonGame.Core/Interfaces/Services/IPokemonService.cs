using PokemonGame.Core.Models.Dtos.RoomBattle;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Settings;

namespace PokemonGame.Core.Interfaces.Services
{
    public interface IPokemonService
    {
        Task<PaginationModel<Pokemon>> GetPokemonAsync(int page, int pageSize, string? namePokemon);
        Task<Pokemon> GetDetailPokemonAsync(int pokemonId);
        Task<List<PokemonTeamDto>> GetRandomPokemons();
    }
}
