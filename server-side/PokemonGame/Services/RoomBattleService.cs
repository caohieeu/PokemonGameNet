using AutoMapper;
using MongoDB.Driver;
using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Dtos.RoomChat;
using PokemonGame.Exceptions;
using PokemonGame.Models;
using PokemonGame.Models.SubModel;
using PokemonGame.Repositories;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Services.IService;

namespace PokemonGame.Services
{
    public class RoomBattleService : IRoomBattleService
    {
        private readonly IRoomBattleRepository _roomBattleRepository;
        private readonly IUserService _userService;
        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;

        private static readonly Dictionary<string, List<string>> _userConnection = new();
        public RoomBattleService(
            IRoomBattleRepository roomBattleRepository, 
            IUserService userService,
            IPokemonService pokemonService,
            IMapper mapper)
        {
            _roomBattleRepository = roomBattleRepository;
            _userService = userService;
            _pokemonService = pokemonService;
            _mapper = mapper;
        }
        public Task AddUserToConnection(string username, string connectionId)
        {
            if (!_userConnection.ContainsKey(username))
                _userConnection[username] = new List<string>();

            _userConnection[username].Add(connectionId);

            return Task.CompletedTask;
        }
        public Task RemoveUserConnection(string username, string connectionId)
        {
            if (_userConnection.ContainsKey(username))
            {
                _userConnection[username].Remove(connectionId);

                if (_userConnection[username].Count == 0)
                    _userConnection.Remove(username);
            }

            return Task.CompletedTask;
        }
        public List<string> GetUserConnections(string username)
        {
            return _userConnection.ContainsKey(username) ? _userConnection[username] : new List<string>();
        }
        public async Task<RoomBattle> AddRoomBattle()
        {
            var room = new RoomBattle
            {
                Participants = new List<ParticipantRoomBattleDto>(),
                Status = "InProgress",
                DateCreated = DateTime.Now,
            };

            await _roomBattleRepository.Add(room);

            return room;
        }

        public async Task<bool> RemoveRoomBattle(string roomId)
        {
            return await _roomBattleRepository.Remove(roomId);
        }

        public async Task<bool> AddParticipant(RandomPokemonDto randomPokemonDto)
        {
            var user = await _userService.GetUserByUsername(randomPokemonDto.UserName);

            if(user == null)
            {
                throw new NotFoundException("User not found");
            }

            var pokemons = await _pokemonService.GetRandomPokemons();

            var participant = new ParticipantRoomBattleDto();
            participant.UserId = user.Id;
            participant.UserName = user.UserName;
            participant.Avatar = user.ImagePath;
            participant.pokemons = pokemons;
            participant.CurrentPokemon = pokemons[0];

            var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, randomPokemonDto.BattleId);
            var room = await _roomBattleRepository.IsExist(filter);

            if (!room)
            {
                throw new NotFoundException("Room battle not found");
            }

            var builder = Builders<RoomBattle>.Update;
            var update = builder.Push(rc => rc.Participants, participant);
            var res = await _roomBattleRepository.UpdateOneByFilter(filter, update);

            return res;
        }

        public async Task<RoomBattle> GetRoomBattle(string roomId)
        {
            var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, roomId);
            var res = await _roomBattleRepository.GetByFilter(filter);
            return res;
        }

        public async Task<bool> SwitchPokemon(SwitchPokemonDto switchPokemon)
        {
            var room = await GetRoomBattle(switchPokemon.RoomId);

            //var pokemon = await GetPokemonRoomBattle(switchPokemon.RoomId, switchPokemon.NewPokemonId);

            var participant = room.Participants
                    .FirstOrDefault(p => p.UserId == switchPokemon.Player);
            if (participant == null) return false;

            var newPokemon = participant.pokemons
                .FirstOrDefault(p => p.Id == switchPokemon.NewPokemonId);
            if (newPokemon == null) return false;

            participant.CurrentPokemon = newPokemon;

            var filter = Builders<RoomBattle>.Filter.Eq(rb => rb.Id, room.Id);
            var builder = Builders<RoomBattle>.Update;
            var update = builder.Set(rb => rb.Participants, room.Participants);

            var result = await _roomBattleRepository.UpdateOneByFilter(filter, update);

            return result;
        }

        public async Task<Pokemon> GetPokemonRoomBattle(string roomId, int pokemonId)
        {
            var filter = Builders<RoomBattle>.Filter.And(
                    Builders<RoomBattle>.Filter.Eq(x => x.Id, roomId),
                    Builders<RoomBattle>.Filter.ElemMatch(
                            x => x.Participants,
                            Builders<ParticipantRoomBattleDto>.Filter.ElemMatch(
                                    p => p.pokemons,
                                    pokemon => pokemon.Id == pokemonId
                                )
                        )
                   );

            var roomBattle = await _roomBattleRepository.GetByFilter(filter);

            if(roomBattle != null )
            {
                var participants = roomBattle.Participants
                    .FirstOrDefault(p => p.pokemons.Any(pkm => pkm.Id == pokemonId));

                if(participants != null)
                {
                    var pokemon = participants.pokemons
                        .FirstOrDefault(p => p.Id == pokemonId);
                    return pokemon;
                }
            }

            return null;
        }
    }
}
