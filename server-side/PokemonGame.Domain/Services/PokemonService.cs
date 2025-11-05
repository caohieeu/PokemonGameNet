using AutoMapper;
using MongoDB.Driver;
using PokemonGame.Core.Models.Dtos.RoomBattle;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.SubModel;
using PokemonGame.Core.Interfaces.Repositories;
using PokemonGame.Core.Interfaces.Services;
using PokemonGame.Core.Settings;
using PokemonGame.Core.Constants;

namespace PokemonGame.Domain.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMoveService _moveService;
        private readonly IMapper _mapper;
        private const string PrefixCacheKey = "pokemon_";
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

        public async Task<PaginationModel<Pokemon>> GetPokemonAsync(int page, int pageSize, string? namePokemon)
        {
            var cacheKey = $"{PrefixCacheKey}list_{page}_{pageSize}_{namePokemon}";

            FilterDefinition<Pokemon> filter = Builders<Pokemon>.Filter.Empty;
            if(!string.IsNullOrEmpty(namePokemon))
                filter = Builders<Pokemon>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(namePokemon, "i"));
            else
                filter = Builders<Pokemon>.Filter.Empty;

            return await _pokemonRepository.GetManyByFilter(page, pageSize, filter, cacheKey);
        }

        public async Task<List<PokemonTeamDto>> GetRandomPokemons()
        {
            List<PokemonTeamDto> pokemons = new List<PokemonTeamDto>();
            Random rnd = new Random();

            for(int i = 0; i < 4; i++)
            {
                var idPokemon = rnd.Next(1, 918);
                var pokemon = await GetDetailPokemonAsync(idPokemon);
                var pokemonTeam = _mapper.Map<PokemonTeamDto>(pokemon);

                var movesRand = await GetRandomMoves(pokemon, 4);
                var statRand = RandomStat(pokemon);

                movesRand.ForEach(x => x.OriginalPP = (int)x.PP);
                pokemonTeam.Moves = movesRand.ToList();
                pokemonTeam.Stat = statRand ?? pokemonTeam.Stat;
                pokemonTeam.OriginalStat = statRand ?? pokemonTeam.OriginalStat;

                pokemons.Add(pokemonTeam);
            }

            return pokemons;
        }
        Stat? RandomStat(Pokemon pokemon)
        {
            if(pokemon == null)
            {
                return null;
            }

            Random rnd = new Random();
            var totalStatLimit = GameDefault.POKEMON_TOTAL_STAT_LIMIT;
            var maxStatLimit = GameDefault.MAX_STAT_LIMIT;
            var totalRemaning = (int)(pokemon.Stat.Atk + pokemon.Stat.Defense +
                pokemon.Stat.SpAtk + pokemon.Stat.SpDef + pokemon.Stat.Speed + totalStatLimit);

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
            //var moveIds = (await _pokemonRepository
            //            .GetFieldValueAsync(x => x.Moves.Select(m => (int)m.Id)))
            //            .SelectMany(ids => ids)
            //            .ToList();
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
