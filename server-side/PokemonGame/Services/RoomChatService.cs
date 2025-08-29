using AutoMapper;
using MongoDB.Driver;
using PokemonGame.Core.Models.Dtos.RoomChat;
using PokemonGame.Exceptions;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.SubModel;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Services.IService;
using PokemonGame.Core.Constants;

namespace PokemonGame.Services
{
    public class RoomChatService : IRoomChatService
    {
        private readonly IRoomChatRepository _roomChatRepository;
        private readonly IMapper _mapper;
        private static readonly Dictionary<string, List<string>> _userConnection = new();
        public RoomChatService(
            IRoomChatRepository roomChatRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _roomChatRepository = roomChatRepository;
        }
        public async Task<bool> AddNewRoom(RoomChatCreateDto roomChatRequestDto)
        {
            var newRoom = _mapper.Map<RoomChat>(roomChatRequestDto);
            newRoom.DateCreated = DateTime.Now;
            newRoom.Participants = new List<Participant>();

            return await _roomChatRepository.Add(newRoom);
        }

        public Task AddUserToConnection(string username, string connectionId)
        {
            if(!_userConnection.ContainsKey(username))
                _userConnection[username] = new List<string>();

            _userConnection[username].Add(connectionId);

            return Task.CompletedTask;
        }
        public Task RemoveUserConnection(string username, string connectionId)
        {
            if(_userConnection.ContainsKey(username))
            {
                _userConnection[username].Remove(connectionId);

                if (_userConnection[username].Count == 0)
                    _userConnection.Remove(username);
            }

            return Task.CompletedTask;
        }
        public async Task<bool> AddUserToRoom(JoinRoomDto joinRoomDto)
        {
            if(joinRoomDto.Participant == null || string.IsNullOrEmpty(joinRoomDto.RoomChatID))
            {
                return false;
            }

            FilterDefinition<RoomChat> filter = 
                Builders<RoomChat>.Filter.Eq(x => x.Id, joinRoomDto.RoomChatID);
            var room = await _roomChatRepository.IsExist(filter);

            if(!room)
            {
                throw new NotFoundException(ExceptionMessage.ROOM_BATTLE_NOT_FOUND);
            }

            if (await IsExistParticipant(joinRoomDto.RoomChatID, joinRoomDto.Participant.UserId))
            {
                return false;
            }

            var builder = Builders<RoomChat>.Update;
            var update = builder.Push(rc => rc.Participants, joinRoomDto.Participant);
            var res = await _roomChatRepository.UpdateOneByFilter(filter, update);

            return res;
        }
        public async Task<bool> RemoveUserFromRoom(RemoveParticipantDto removeParticipantDto)
        {
            FilterDefinition<RoomChat> filter =
                Builders<RoomChat>.Filter.Eq(x => x.Id, removeParticipantDto.RoomChatID);
            var room = await _roomChatRepository.IsExist(filter);

            if (!room)
            {
                throw new NotFoundException(ExceptionMessage.ROOM_CHAT_NOT_FOUND);
            }

            var builder = Builders<RoomChat>.Update;
            var update = builder.PullFilter(
                rc => rc.Participants,
                participant => participant.UserId == removeParticipantDto.UserId
            );
            var res = await _roomChatRepository.UpdateOneByFilter(filter, update);

            return res;
        }
        public List<string> GetUserConnections(string username)
        {
            return _userConnection.ContainsKey(username) ? _userConnection[username] : new List<string>();
        }

        public async Task<List<RoomChat>> GetAllRoomChat()
        {
            FilterDefinition<RoomChat> filter = Builders<RoomChat>.Filter.Empty;
            var res = await _roomChatRepository.GetAll(filter);
            return res.ToList();
        }

        public async Task<bool> IsExistParticipant(string roomId, string userId)
        {
            var filter = Builders<RoomChat>.Filter.And(
                Builders<RoomChat>.Filter.Eq(rc => rc.Id, roomId),
                Builders<RoomChat>.Filter.ElemMatch(
                rc => rc.Participants,
                participant => participant.UserId == userId
             )
            );

            return await _roomChatRepository.IsExist(filter);
        } 

        public async Task<List<Participant>> GetParticipants(string roomId)
        {

            var room = await GetRoomChat(roomId);
            return room.Participants;
        }

        public async Task<RoomChat> GetRoomChat(string roomId)
        {
            FilterDefinition<RoomChat> filter = Builders<RoomChat>.Filter.Eq(x => x.Id, roomId);
            return await _roomChatRepository.GetByFilter(filter);
        }
    }
}

