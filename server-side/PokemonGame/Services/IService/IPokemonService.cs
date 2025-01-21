using PokemonGame.Models;
using PokemonGame.Settings;

namespace PokemonGame.Services.IService
{
    public interface IPokemonService
    {
        Task<PaginationModel<Pokemon>> GetPokemonAsync(int page, int pageSize, string namePokemon);
        Task<Pokemon> GetDetailPokemonAsync(int pokemonId);
        Task<List<Pokemon>> GetRandomPokemons();
    }
}
