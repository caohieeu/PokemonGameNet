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
using PokemonGame.Utils.Global;

namespace PokemonGame.Services
{
    public class RoomBattleService : IRoomBattleService
    {
        private readonly IRoomBattleRepository _roomBattleRepository;
        private readonly IUserService _userService;
        private readonly IPokemonService _pokemonService;
        private readonly IMoveService _moveService;
        private readonly IMapper _mapper;

        private static readonly Dictionary<string, List<string>> _userConnection = new();
        public RoomBattleService(
            IRoomBattleRepository roomBattleRepository, 
            IUserService userService,
            IPokemonService pokemonService,
            IMoveService moveService,
            IMapper mapper)
        {
            _roomBattleRepository = roomBattleRepository;
            _userService = userService;
            _pokemonService = pokemonService;
            _moveService = moveService;
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
        public string GetUserFromConnection(string connectionId)
        {
            var user = _userConnection.FirstOrDefault(x => x.Value.Any(x => x == connectionId));
            return user.Key;
        }
        public async Task<RoomBattle> AddRoomBattle()
        {
            var room = new RoomBattle
            {
                Participants = new List<ParticipantRoomBattleDto>(),
                Status = "InProgress",
                ActionLog = new ActionLog(),
                CurrentTurn = "",
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
            participant.Status = Utils.Global.ParticipantStatus.InRoom;
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

            var participant = room.Participants
                    .FirstOrDefault(p => p.UserId == switchPokemon.Player);
            if (participant == null) return false;

            var newPokemon = participant.pokemons
                .FirstOrDefault(p => p.Id == switchPokemon.NewPokemonId);
            if (newPokemon == null) return false;

            if(newPokemon.Stat.Hp <= 0) return false;

            participant.CurrentPokemon = newPokemon;

            var filter = Builders<RoomBattle>.Filter.Eq(rb => rb.Id, room.Id);
            var builder = Builders<RoomBattle>.Update;
            var update = builder.Set(rb => rb.Participants, room.Participants);

            var result = await _roomBattleRepository.UpdateOneByFilter(filter, update);

            return result;
        }

        public async Task<PokemonTeamDto> GetPokemonRoomBattle(string roomId, int pokemonId)
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

        public async Task<bool> UpdateStatusRoomBattle(string roomId, string status)
        {
            var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, roomId);

            var builder = Builders<RoomBattle>.Update;
            var update = builder.Set(x => x.Status, status);

            var result = await _roomBattleRepository.UpdateOneByFilter(filter, update);

            return result;
        }

        public async Task<bool> RemoveUserFromRoom(string roomId, string username)
        {
            var room = await GetRoomBattle(roomId);

            var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, roomId);
            var builder = Builders<RoomBattle>.Update;
            var update = builder.PullFilter(
                x => x.Participants,
                participant => participant.UserName == username
            );
            var res = await _roomBattleRepository.UpdateOneByFilter(filter, update);

            return res;
        }

        public async Task<ParticipantRoomBattleDto> GetUserFromRoomBattle(string roomId, string username)
        {
            var room = await GetRoomBattle(roomId);

            var user = room.Participants.First(x => x.UserName == username);

            return user;
        }

        public async Task<bool> UpdateStatusParticipant(string roomId, string username, string status)
        {
            var room = await GetRoomBattle(roomId);

            if(room != null)
            {
                var participantToUpdate = room.Participants.FirstOrDefault(x => x.UserName == username);

                if(participantToUpdate != null)
                    participantToUpdate.Status = status;

                var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, roomId);
                var builder = Builders<RoomBattle>.Update;
                var update = builder.Set(x => x.Participants, room.Participants);

                var res = await _roomBattleRepository.UpdateOneByFilter(filter, update);

                return res;
            } 

            return false;
        }

        public async Task ExcuteWinner(string roomId, string userId)
        {
            var room = await GetRoomBattle(roomId);

            room.Winner = userId;
            room.Status = "Completed";

            var res = await UpdateRoomBattle(room);
            if (!res)
                Console.WriteLine("update room battle error");
        }

        public async Task<bool> UpdateCurrentTurn(string roomId, string username)
        {
            var room = await GetRoomBattle(roomId);

            var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, roomId);
            var builder = Builders<RoomBattle>.Update;
            var update = builder.Set(x => x.CurrentTurn, username);

            return await _roomBattleRepository.UpdateOneByFilter(filter, update);
        }

        public async Task<BattleResultDto> ApplyMove(PokemonTeamDto attacker, PokemonTeamDto defender, MoveStateDto move)
        {
            var result = new BattleResultDto()
            {
                Attacker = attacker.Name,
                Defender = defender.Name,
                IdMoveUsed = move.Id
            };

            double damage = Utils.Calculate.CalculateDamage(100, move.Power, attacker.Stat.Atk,
                defender.Stat.Defense, Utils.Calculate.calculateStab(attacker.Type, move.Type),
                Utils.TypeEffectiveness.GetEffectiveness(attacker.Type.First(), defender.Type), false);

            //var cateMove = _moveService.FilterMove(move.Effect);

            //if (!Enum.TryParse(cateMove.Effect, out CategoryMove cate))
            //{
            //    return null;
            //}

            //switch (cate)
            //{
            //    case CategoryMove.NormalDamage:
            //        defender = _moveService.ProcessNormalMove(attacker, defender, move);
            //        break;
            //    default:
            //        Console.WriteLine($"Move type {cate} is not handled yet.");
            //        break;
            //}

            result.DamageDealt = (int)damage;
            result.DefenderHP = (int)defender.Stat.Hp - (int)damage;

            return result;
        }

        public async Task<bool> SwitchRemainingPokemon(string roomId, string playerId, List<PokemonTeamDto> pokemons)
        {
            foreach(var pokemon in pokemons)
            {
                if(pokemon.Stat.Hp > 0)
                {
                    var switchPkm = new SwitchPokemonDto()
                    {
                        RoomId = roomId,
                        Player = playerId,
                        NewPokemonId = pokemon.Id,
                    };

                    await SwitchPokemon(switchPkm);
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UpdateRoomBattle(RoomBattle roomBattle)
        {
            var filter = Builders<RoomBattle>.Filter.Eq(x => x.Id, roomBattle.Id);
            return await _roomBattleRepository.ReplaceOneAsync(filter, roomBattle);
        }

        public async Task<ParticipantRoomBattleDto> GetParticipant(string roomId, string username)
        {
            var room = await GetRoomBattle(roomId);

            return room.Participants.First(x => x.UserName == username);
        }
    }
}
