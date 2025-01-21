using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PokemonGame.DAL;
using PokemonGame.Dtos.Response;
using PokemonGame.Dtos.RoomChat;
using PokemonGame.Exceptions;
using PokemonGame.Models.SubModel;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace PokemonGame.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRoomChatService _roomChatService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        public ChatHub(
            IRoomChatService roomChatService, 
            IUserService userService,
            IHttpContextAccessor contextAccessor,
            IUserContext userContext,
            IMapper mapper)
        {
            _roomChatService = roomChatService;
            _userService = userService;
            _contextAccessor = contextAccessor;
            _userContext = userContext;
            _mapper = mapper;
        }
        public async Task SendMessageToAllUser(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
        public async Task SendMessageToUsers(string username, string message)
        {
            var connections = _roomChatService.GetUserConnections(username);
            foreach(var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", username, message);
            }
        }
        public async Task SendMessageToGroup(string roomId, string username, string message)
        {
            var roomReceiveId = roomId;
            await Clients.Group(roomId).SendAsync("ReceiveMessageGroup", username, message, roomReceiveId);
        }
        public async Task JoinGroup(string roomId)
        {
            var user = await GetUserFromContext();

            if (user == null)
            {
                return;
            }

            await _roomChatService.AddUserToConnection(user.UserName, Context.ConnectionId);

            var joinRoomDto = new JoinRoomDto();
            var participant = new Participant
            {
                UserId = user.Id,
                UserName = user.UserName,
                Avatar = user.ImagePath,
            };
            joinRoomDto.RoomChatID = roomId;
            joinRoomDto.Participant = participant;

            var userConnection = _roomChatService.GetUserConnections(user.UserName);
            foreach (var connection in userConnection)
            {
                await Groups.AddToGroupAsync(connection, roomId);
            }

            if (!await _roomChatService.AddUserToRoom(joinRoomDto))
            {
                return;
            }

            await Clients.Group(roomId).SendAsync("UserJoined", new
            {
                userName = user.UserName,
                avatar = user.ImagePath
            });
        }
        public async Task ExitGroup(string roomId)
        {
            var user = await GetUserFromContext();

            if (user == null)
            {
                return;
            }

            await _roomChatService.RemoveUserConnection(user.UserName, Context.ConnectionId);

            var userResponse = new
            {
                userName = user.UserName,
                avatar = user.ImagePath
            };

            var rmParticipant = new RemoveParticipantDto();
            rmParticipant.RoomChatID = roomId;
            rmParticipant.UserId = user.Id;

            var userConnection = _roomChatService.GetUserConnections(user.Id);
            foreach (var connection in userConnection)
            {
                await Groups.RemoveFromGroupAsync(connection, roomId);
            }

            await _roomChatService.RemoveUserFromRoom(rmParticipant);

            await Clients.Group(roomId).SendAsync("UserDisconnectedGroup", user.UserName);
        }
        public async Task<InfoUserResponseDto> GetUserFromContext()
        {
            var user = new InfoUserResponseDto();

            var httpContext = Context.GetHttpContext();
            var token = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "")
                       ?? httpContext?.Request.Query["access_token"];

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token not found in Authorization header.");
            }

            if (!_userContext.CheckToken(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserName")?.Value;
            if (string.IsNullOrEmpty(username))
            {
                throw new NotFoundException("UserName is not found in token claims");
            }

            user = await _userService.GetUserByUsername(username);

            return user;
        }
        public override async Task OnConnectedAsync()
        {
            var user = new InfoUserResponseDto();

            try
            {
                user = await GetUserFromContext();

                if (user == null)
                {
                    return;
                }
            }
            catch
            {
                await base.OnConnectedAsync();
                return;
            }

            var userResponse = new
            {
                userName = user.UserName,
                avatar = user.ImagePath
            };

            //await _roomChatService.AddUserToConnection(user.UserName, Context.ConnectionId);

            //add user connected to roomchat
            var allRoom = await _roomChatService.GetAllRoomChat();
            var joinRoomDto = new JoinRoomDto();
            joinRoomDto.RoomChatID = allRoom
                .Where(x => x.Name == "Lobby")
                .Select(x => x.Id)
                .FirstOrDefault() ?? "";
            var participant = new Participant
            {
                UserId = user.Id,
                UserName = user.UserName,
                Avatar = user.ImagePath,
            };
            joinRoomDto.Participant = participant;

            if(!await _roomChatService.AddUserToRoom(joinRoomDto))
            {
                return;
            }

            await Clients.All.SendAsync("UserConnected", userResponse);

            await base.OnConnectedAsync();
        }
    
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await GetUserFromContext();

            //await _roomChatService.RemoveUserConnection(user.UserName, Context.ConnectionId);

            var allRoom = await _roomChatService.GetAllRoomChat();
            var rmParticipant = new RemoveParticipantDto();
            rmParticipant.RoomChatID = allRoom
                .Where(x => x.Name == "Lobby")
                .Select(x => x.Id)
                .FirstOrDefault() ?? "";
            rmParticipant.UserId = user.Id;

            await _roomChatService.RemoveUserFromRoom(rmParticipant);

            await Clients.All.SendAsync("UserDisconnected", user.UserName);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
