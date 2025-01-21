using Microsoft.AspNetCore.SignalR;
using PokemonGame.DAL;
using PokemonGame.Dtos.Response;
using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Exceptions;
using PokemonGame.Services.IService;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace PokemonGame.Hubs
{
    public class GameHub : Hub
    {
        private readonly IUserContext _userContext;
        private readonly IUserService _userService;
        private readonly IRoomBattleService _roomBattleService;

        private static List<InfoUserResponseDto> waittingList = new List<InfoUserResponseDto>();
        private static readonly ConcurrentDictionary<string, string> UserRoomMapping = new();

        public GameHub(
            IUserContext userContext, 
            IUserService userService,
            IRoomBattleService roomBattleService)
        {
            _userContext = userContext;
            _userService = userService;
            _roomBattleService = roomBattleService;
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

        public async Task FindMatch()
        {
            var user = await GetUserFromContext();

            if (user == null)
                throw new HubException("You are not logged in yet");

            lock (waittingList)
            {

                _roomBattleService.AddUserToConnection(user.UserName, Context.ConnectionId);
                if (waittingList.Any(x => x.UserName == user.UserName))
                    return;
                waittingList.Add(user);
            }

            await CheckForMatch();
        }
        public async Task CheckForMatch()
        {
            InfoUserResponseDto player1 = null;
            InfoUserResponseDto player2 = null;

            lock (waittingList)
            {
                if(waittingList.Count >= 2)
                {
                    player1 = waittingList[0];
                    player2 = waittingList[1];

                    waittingList.RemoveRange(0, 2);
                }
            }

            if(player1 != null && player2 != null)
            {
                var room = await _roomBattleService.AddRoomBattle();

                await Groups.AddToGroupAsync(player1.UserName, room.Id);
                await Groups.AddToGroupAsync(player2.UserName, room.Id);

                var participant1 = new RandomPokemonDto
                {
                    UserName = player1.UserName,
                    BattleId = room.Id,
                };
                var participant2 = new RandomPokemonDto
                {
                    UserName = player2.UserName,
                    BattleId = room.Id,
                };

                var res1 = await _roomBattleService.AddParticipant(participant1);
                var res2 = await _roomBattleService.AddParticipant(participant2);

                if (!res1 && !res2)
                {
                    return;
                }

                var response = new
                {   
                    RoomId = room.Id,
                    Player1 = player1.UserName,
                    Player2 = player2.UserName,
                };

                var connectionsPlayer1 = _roomBattleService.GetUserConnections(player1.UserName);
                var connectionsPlayer2 = _roomBattleService.GetUserConnections(player2.UserName);

                foreach (var connection in connectionsPlayer1)
                {
                    UserRoomMapping[connection] = room.Id;
                    await Clients.Client(connection).SendAsync("MatchFound", response);
                }
                foreach (var connection in connectionsPlayer2)
                {
                    UserRoomMapping[connection] = room.Id;
                    await Clients.Client(connection).SendAsync("MatchFound", response);
                }
            }
        }
        public async Task HandleUserDisconnectFromRoom(string roomId, string connectionId)
        {
            var room = await _roomBattleService.GetRoomBattle(roomId);

            room.Status = "Completed";

            await Task.CompletedTask;
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            if(UserRoomMapping.TryRemove(connectionId, out var roomId)) {
                await HandleUserDisconnectFromRoom(roomId, connectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
