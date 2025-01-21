using MongoDB.Driver;
using PokemonGame.Models;
using PokemonGame.Models.SubModel;
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

        public async Task<List<Pokemon>> GetRandomPokemons()
        {
            List<Pokemon> pokemons = new List<Pokemon>();
            Random rnd = new Random();

            for(int i = 0; i < 4; i++)
            {
                var idPokemon = rnd.Next(1, 919);
                var pokemon = await GetDetailPokemonAsync(idPokemon);

                pokemon.Moves = GetRandomMoves(pokemon, 4);
                pokemon.Stat = RandomStat(pokemon);

                pokemons.Add(pokemon);
            }

            return pokemons;
        }
        Stat RandomStat(Pokemon pokemon)
        {
            Random rnd = new Random();
            var totalStat = Utils.Global.Global.TotalStat;
            var maxStatLimit = 252;
            var totalRemaning = (int)(pokemon.Stat.Atk + pokemon.Stat.Defense +
                pokemon.Stat.SpAtk + pokemon.Stat.SpDef + pokemon.Stat.Speed + totalStat);

            pokemon.Stat.Hp = rnd.Next(0, Math.Min(totalRemaning, maxStatLimit));
            totalRemaning -= (int)pokemon.Stat.Hp;

            pokemon.Stat.Atk = rnd.Next(0, Math.Min(totalRemaning, maxStatLimit));
            totalRemaning -= (int)pokemon.Stat.Atk;

            pokemon.Stat.Defense = rnd.Next(0, Math.Min(totalRemaning, maxStatLimit));
            totalRemaning -= (int)pokemon.Stat.Defense;

            pokemon.Stat.SpAtk = rnd.Next(0, Math.Min(totalRemaning, maxStatLimit));
            totalRemaning -= (int)pokemon.Stat.SpAtk;

            pokemon.Stat.SpDef = rnd.Next(0, Math.Min(totalRemaning, maxStatLimit));
            totalRemaning -= (int)pokemon.Stat.SpDef;

            pokemon.Stat.Speed = totalRemaning;

            return pokemon.Stat;
        }
        List<MovesPokemon> GetRandomMoves(Pokemon pokemon, int count)
        {
            var moveIds = pokemon.Moves.Select(x => (int)x.Id).ToList();
            Random rand = new Random();
            HashSet<int> ids = new HashSet<int>();
            List<MovesPokemon> result = new List<MovesPokemon>();

            while(ids.Count < moveIds.Count && ids.Count < count)
            {
                int index = rand.Next(0, moveIds.Count);
                if (ids.Add(moveIds[index]))
                {
                    result.Add(pokemon.Moves[index]);
                }
            }

            return result;
        }
    }
}
