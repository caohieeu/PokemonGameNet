using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using PokemonGame.DAL;
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
        public async Task SendMessageToGroup(string group, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", message);
        }
        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var token = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "")
                       ?? httpContext?.Request.Query["access_token"];

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token not found in Authorization header.");
            }

            if(!_userContext.CheckToken(token))
            {
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserName")?.Value;
            if(string.IsNullOrEmpty(username))
            {
                throw new NotFoundException("UserName is not found in token claims");
            }

            var user = await _userService.GetUserByUsername(username);

            var userResponse = new
            {
                userName = username,
                avatar = user.ImagePath
            };

            await _roomChatService.AddUserToConnection(username, Context.ConnectionId);

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
                UserName = username,
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
            var httpContext = Context.GetHttpContext();
            var token = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "")
                       ?? httpContext?.Request.Query["access_token"];

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token not found in Authorization header.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserName")?.Value;
            if (string.IsNullOrEmpty(username))
            {
                throw new NotFoundException("UserName is not found in token claims");
            }

            var user = await _userService.GetUserByUsername(username);

            await _roomChatService.RemoveUserConnection(username, Context.ConnectionId);

            var allRoom = await _roomChatService.GetAllRoomChat();
            var rmParticipant = new RemoveParticipantDto();
            rmParticipant.RoomChatID = allRoom
                .Where(x => x.Name == "Lobby")
                .Select(x => x.Id)
                .FirstOrDefault() ?? "";
            rmParticipant.UserId = user.Id;

            await _roomChatService.RemoveUserFromRoom(rmParticipant);

            await Clients.All.SendAsync("UserDisconnected", username);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
