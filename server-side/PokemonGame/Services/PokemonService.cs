using MongoDB.Driver;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Services.IService;
using PokemonGame.Settings;

namespace PokemonGame.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        public PokemonService(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        public async Task<Pokemon> GetDetailPokemonAsync(int pokemonId)
        {
            var filter = Builders<Pokemon>.Filter.Eq(x => x.Id, pokemonId);

            return await _pokemonRepository.GetByFilter(filter);
        }

        public async Task<PaginationModel<Pokemon>> GetPokemonAsync(int page, int pageSize, string namePokemon)
        {
            FilterDefinition<Pokemon> filter = null;
            if(namePokemon != null)
                filter = Builders<Pokemon>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(namePokemon, "i"));
            else
                filter = Builders<Pokemon>.Filter.Empty;

            return await _pokemonRepository.GetManyByFilter(page, pageSize, filter, Builders<Pokemon>.Sort.Ascending(x => x.Id));
        }
    }
}
