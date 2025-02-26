using Microsoft.AspNetCore.SignalR;
using PokemonGame.DAL;
using PokemonGame.Dtos.Response;
using PokemonGame.Dtos.RoomBattle;
using PokemonGame.Exceptions;
using PokemonGame.Models;
using PokemonGame.Models.SubModel;
using PokemonGame.Services;
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
        private readonly IMoveService _moveService;

        private static List<InfoUserResponseDto> waittingList = new List<InfoUserResponseDto>();
        private static readonly ConcurrentDictionary<string, string> UserRoomMapping = new();

        public GameHub(
            IUserContext userContext, 
            IUserService userService,
            IRoomBattleService roomBattleService,
            IMoveService moveService)
        {
            _userContext = userContext;
            _userService = userService;
            _roomBattleService = roomBattleService;
            _moveService = moveService;
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
                //if (waittingList.Any(x => x.UserName == user.UserName))
                //    return;
                waittingList.Add(user);
            }

            await CheckForMatch();
        }
        async Task ReloadGroup(string userName1, string userName2, string roomId)
        {
            var userConnection1 = _roomBattleService.GetUserConnections(userName1);
            var userConnection2 = _roomBattleService.GetUserConnections(userName2);

            foreach (var conn in userConnection1)
            {
                await Groups.AddToGroupAsync(conn, roomId);
            }
            foreach (var conn in userConnection2)
            {
                await Groups.AddToGroupAsync(conn, roomId);
            }
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

                var userConnection1 = _roomBattleService.GetUserConnections(player1.UserName);
                var userConnection2 = _roomBattleService.GetUserConnections(player2.UserName);
                
                foreach(var conn in userConnection1)
                {
                    await Groups.AddToGroupAsync(conn, room.Id);
                }
                foreach (var conn in userConnection2)
                {
                    await Groups.AddToGroupAsync(conn, room.Id);
                }

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

                var updateCurrentTurn = await _roomBattleService.UpdateCurrentTurn(room.Id, player1.UserName);
                    
                if (!res1 && !res2 && !updateCurrentTurn)
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
        public void ExecuteTurn2(ExecuteTurnDto executeTurn)
        {
            Console.WriteLine($"ExecuteTurn called: {executeTurn.roomId}, {executeTurn.type}");
        }
        [HubMethodName("ExecuteTurn")]
        public async Task ExecuteTurn(ExecuteTurnDto executeTurn)
        {
            var room = await _roomBattleService.GetRoomBattle(executeTurn.roomId);

            var participant = room.Participants.FirstOrDefault(x => x.UserName == executeTurn.usernamePlayer);
            if (participant == null) throw new NotFoundException("Player not found");

            if (room.ActionQueue.Any(x => x.Player == executeTurn.usernamePlayer))
                throw new Exception("You have already chosen an action!");

            if (executeTurn.type == "Switch")
            {
                room.ActionQueue.Add(new ActionQueueDto
                {
                    Player = executeTurn.usernamePlayer,
                    ActionType = "Switch",
                    NewPokemonId = executeTurn.newPokemon,
                });
                await _roomBattleService.UpdateRoomBattle(room);
            }
            else if(executeTurn.type == "Attack")
            {
                var move = await _moveService.GetMove(executeTurn.moveId);
                var moveSd = _moveService.TransformMove(move);

                room.ActionQueue.Add(new ActionQueueDto
                {
                    Player = executeTurn.usernamePlayer,
                    ActionType = "Attack",
                    MoveId = executeTurn.moveId,
                    Speed = (int)participant.CurrentPokemon.Stat.Speed,
                });
                await _roomBattleService.UpdateRoomBattle(room);
            }
            else
            {
                Console.WriteLine("Dau hang");
            }

            if(room.ActionQueue.Count == 2)
            {
                await ResolveTurn(room);
            }
        }
        public async Task ResolveTurn(RoomBattle room)
        {
            await ReloadGroup(room.Participants[0].UserName, room.Participants[1].UserName, room.Id);

            var actions = room.ActionQueue.OrderByDescending(x => x.Speed)
                .ToList();
            
            foreach(var action in actions)
            {
                var attacker = room.Participants.FirstOrDefault(x => x.UserName == action.Player);
                var defender = room.Participants.FirstOrDefault(x => x.UserName != action.Player);

                if(action.ActionType == "Switch")
                {
                    await SwitchPokemon(room, action.NewPokemonId, action.Player);

                    await Clients.Group(room.Id).SendAsync("SwitchPokemon", defender.UserName);
                }
                else if(action.ActionType == "Attack")
                {
                    var move = await _moveService.GetMove(action.MoveId);
                    var moveSd = _moveService.TransformMove(move);

                    Random random = new Random();
                    if(random.Next(100) >= moveSd.Accuracy)
                    {
                        await Clients.Group(room.Id).SendAsync("MissedAttack", attacker.UserName, moveSd.Name);
                        continue;
                    }

                    var battleResult = await _roomBattleService.ApplyMove(attacker.CurrentPokemon, defender.CurrentPokemon, moveSd);

                    if (battleResult == null)
                    {
                        Console.WriteLine("Battle Result is null");
                        return;
                    }

                    var currRoom = await _roomBattleService.GetRoomBattle(room.Id);
                    var participantDef = currRoom.Participants.First(x => x.UserName == defender.UserName);
                    var participantAtk = currRoom.Participants.First(x => x.UserName == attacker.UserName);

                    participantDef.CurrentPokemon.Stat.Hp = Math.Max(0, battleResult.DefenderHP);
                    participantDef.pokemons = participantDef.pokemons
                        .Select(x => x.Name == battleResult.Defender ? participantDef.CurrentPokemon : x)
                        .ToList();

                    participantAtk.CurrentPokemon.Moves = participantAtk.CurrentPokemon.Moves
                        .Select(x => x.Id == battleResult.IdMoveUsed ? new MoveStateDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Type = x.Type,
                            Power = x.Power,
                            PP = --x.PP,
                            OriginalPP = x.OriginalPP,
                            Accuracy = x.Accuracy,
                            Effect = x.Effect,
                            ShortEffect = x.ShortEffect,
                            MoveData = x.MoveData,
                        } : x)
                        .ToList();
                    participantAtk.pokemons
                        .Select(x => x.Name == battleResult.Attacker ? participantAtk.CurrentPokemon : x)
                        .ToList();

                    await _roomBattleService.UpdateRoomBattle(currRoom);

                    await Clients.Group(room.Id).SendAsync("ReceiveBattleResult", battleResult);

                    if(battleResult.DefenderHP <= 0)
                    {
                        if (await _roomBattleService.SwitchRemainingPokemon(room.Id, defender.UserId, participantDef.pokemons))
                        {   
                            await Clients.Group(room.Id).SendAsync("SwitchPokemon", defender.UserName);
                        }
                        else
                        {
                            await Clients.Group(room.Id).SendAsync("Finished", room.Id, attacker.UserName);
                            await _roomBattleService.ExcuteWinner(room.Id, attacker.UserName, defender.UserName);
                            return;
                        }
                    }
                }
            }

            var roomUpdate = await _roomBattleService.GetRoomBattle(room.Id);
            roomUpdate.ActionQueue.Clear();
            await _roomBattleService.UpdateRoomBattle(roomUpdate);
        }
        public async Task SwitchPokemon(RoomBattle room, int newPokemon, string usernamePlayer)
        {
            var participant = room.Participants.First(x => x.UserName == usernamePlayer);

            var switchPokemon = new SwitchPokemonDto()
            {
                RoomId = room.Id,
                Player = participant.UserId,
                NewPokemonId = newPokemon,
            };

            await _roomBattleService.SwitchPokemon(switchPokemon);
        }
        public async Task Attack(string roomId, int moveId)
        {
            var room = await _roomBattleService.GetRoomBattle(roomId);

            var connectionId = Context.ConnectionId;
            var usernamePlayer = _roomBattleService.GetUserFromConnection(connectionId);
            var participant = room.Participants.FirstOrDefault(x => x.UserName == usernamePlayer);

            if(participant == null)
            {
                throw new NotFoundException($"{usernamePlayer} is not found in room");
            }

            if(room.CurrentTurn != participant.UserName)
            {
                throw new Exception("Not your turn");
            }

            var attacker = room.Participants.FirstOrDefault(x => x.UserName == participant.UserName);
            var defender = room.Participants.FirstOrDefault(x => x.UserName != participant.UserName);

            if(attacker == null && defender == null)
                throw new Exception("Invalid participant");

            var move = await _moveService.GetMove(moveId);
            var moveSd = _moveService.TransformMove(move);

            if (moveSd == null)
                throw new Exception("Invalid move");

            var battleResult = await _roomBattleService.ApplyMove(attacker?.CurrentPokemon, defender?.CurrentPokemon, moveSd);

            if(battleResult.DefenderHP <= 0)
            {
                if(await _roomBattleService.SwitchRemainingPokemon(roomId, defender.UserId, defender.pokemons))
                {
                    await Clients.Group(roomId).SendAsync("SwitchPokemon", defender.UserName);
                }
                else
                {
                    await Clients.Group(roomId).SendAsync("Finished", roomId, attacker.UserName);
                }
            }

            await Clients.Group(roomId).SendAsync("ReceiveBattleResult", battleResult);
        }
        public async Task HandleUserDisconnectFromRoom(string connectionId)
        {
            var username = _roomBattleService.GetUserFromConnection(connectionId);

            var connections = _roomBattleService.GetUserConnections(username).ToList();
            foreach (var connection in connections)
            {
                await _roomBattleService.RemoveUserConnection(username, connection);
            }

            await Task.CompletedTask;
        }
        public override async Task OnConnectedAsync()
        {
            var currUser = await GetUserFromContext();
            var connectionId = Context.ConnectionId;

            await _roomBattleService.AddUserToConnection(currUser.UserName, connectionId);

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var currUser = await GetUserFromContext();

            var connections = _roomBattleService.GetUserConnections(currUser.UserName).ToList();

           foreach(var connectionId in connections)
            {
                await HandleUserDisconnectFromRoom(connectionId);
            } 
           
            //if (UserRoomMapping.TryRemove(connectionId, out var roomId))
            //{
            //    await HandleUserDisconnectFromRoom(connectionId);
            //}  

            await base.OnDisconnectedAsync(exception);
        }
    }
}
