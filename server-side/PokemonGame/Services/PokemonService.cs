using AutoMapper;
using MongoDB.Driver;
using PokemonGame.Dtos.RoomBattle;
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
        private readonly IMoveService _moveService;
        private readonly IMapper _mapper;
        public PokemonService(
            IPokemonRepository pokemonRepository, 
            IMoveService moveService,
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _moveService = moveService;
            _mapper = mapper;
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

        public async Task<List<PokemonTeamDto>> GetRandomPokemons()
        {
            List<PokemonTeamDto> pokemons = new List<PokemonTeamDto>();
            Random rnd = new Random();

            for(int i = 0; i < 4; i++)
            {
                var idPokemon = rnd.Next(1, 919);
                var pokemon = await GetDetailPokemonAsync(idPokemon);
                var pokemonTeam = _mapper.Map<PokemonTeamDto>(pokemon);

                var movesRand = await GetRandomMoves(pokemon, 4);
                var statRand = RandomStat(pokemon);

                movesRand.ForEach(x => x.OriginalPP = (int)x.PP);
                pokemonTeam.Moves = movesRand.ToList();
                pokemonTeam.Stat = statRand;
                pokemonTeam.OriginalStat = statRand;

                pokemons.Add(pokemonTeam);
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
        async Task<List<MoveStateDto>> GetRandomMoves(Pokemon pokemon, int count)
        {
            var moveIds = pokemon.Moves.Select(x => (int)x.Id).ToList();
            Random rand = new Random();
            HashSet<int> ids = new HashSet<int>();
            List<MoveStateDto> result = new List<MoveStateDto>();

            while(ids.Count < moveIds.Count && ids.Count < count)
            {
                int index = rand.Next(1, moveIds.Count);
                if (ids.Add(moveIds[index]))
                {
                    MovesPokemon movePk = pokemon.Moves[index];
                    Moves move = await _moveService.GetMove(index);
                    MoveStateDto moveSd = _moveService.TransformMove(move);

                    result.Add(moveSd);
                }
            }

            return result;
        }
    }
}
