using MongoDB.Driver;
using PokemonGame.Core.Models.Dtos.Auth;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Services.IService;
using PokemonGame.Exceptions;
using Microsoft.AspNetCore.Identity;
using PokemonGame.Core.Models.Dtos.Response;
using PokemonGame.DAL;
using AutoMapper;
using PokemonGame.Core.Models.SubModel;
using PokemonGame.Core.Models.Dtos.Pokemon;
using MongoDB.Bson;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Constants;

namespace PokemonGame.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        private readonly IPokemonRepository _pokeRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IMapper _mapper;
        public UserService(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            IPokemonRepository pokeRepository,
            IMoveRepository moveRepository,
            IUserContext userContext,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userContext = userContext;
            _pokeRepository = pokeRepository;
            _moveRepository = moveRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUser()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }
        public async Task<ApplicationUser> GetUserByUsernameAndPassword(SignInDto user)
        {
            var filter = Builders<ApplicationUser>.Filter.And(
                Builders<ApplicationUser>.Filter.Eq(x => x.UserName , user.username),
                Builders<ApplicationUser>.Filter.Eq(x => x.PasswordHash, user.password)
            );
            return await _userRepository.GetByFilter(filter);
        }
        public async Task<AuthDto> SignIn(SignInDto user)
        {
            return await _userRepository.SignIn(user);
        }

        public Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar)
        {
            return _userRepository.SignUp(signUpDto, avatar);
        }

        public Task<ApplicationUser> GetUserByUsernameAndPassword()
        {
            throw new NotImplementedException();
        }

        public async Task<InfoUserResponseDto> GetInfoUser()
        {
            return await _userContext.GetCurrentUserInfo();
        }

        public async Task<bool> AddNewTeamPokemon(string userId, AddTeamPokemonDto teamPokemon)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if(user.Teams.Count == 0)
            {
                user.Teams = new List<TeamPokemonDto>();
            }

            var team = new TeamPokemonDto();
            var listPokemon = new List<Pokemon>();

            foreach(var poke in teamPokemon.Pokemons)
            {
                FilterDefinition<Pokemon> filter = Builders<Pokemon>.Filter.Eq(x => x.Id, poke.PokemonId);
                var pokemon = await _pokeRepository.GetByFilter(filter);
                if (pokemon == null)
                {
                    throw new NotFoundException("Pokemon not found");
                }

                var additionStat = poke.Stat.Hp + poke.Stat.Atk +
                poke.Stat.Defense + poke.Stat.SpAtk +
                poke.Stat.SpDef + poke.Stat.Speed;

                if (additionStat > GameDefault.POKEMON_TOTAL_STAT_LIMIT)
                {
                    throw new BadRequestException("Exceeding the permissible limit");
                }

                pokemon.Stat.Hp += poke.Stat.Hp;
                pokemon.Stat.Atk += poke.Stat.Atk;
                pokemon.Stat.Defense += poke.Stat.Defense;
                pokemon.Stat.SpAtk += poke.Stat.SpAtk;
                pokemon.Stat.SpDef += poke.Stat.SpDef;
                pokemon.Stat.Speed += poke.Stat.Speed;
                pokemon.Moves = new List<MovesPokemon>();

                foreach (var moveId in poke.MoveIds)
                {
                    FilterDefinition<Moves> filterMove = Builders<Moves>.Filter.Eq(m => m.Id, moveId);
                    var move = await _moveRepository.GetByFilter(filterMove);
                    
                    if (move == null)
                    {
                        throw new NotFoundException($"{move?.Name} does not exist");
                    }

                    var movePoke = new MovesPokemon
                    {
                        Id = move.Id,
                        Name = move.Name,
                        Type = new Species() { Name = move.Type },
                        Power = move.Power,
                        PP = move.PP,
                        Accuracy = move.Accuracy,
                    };
                    pokemon.Moves.Add(movePoke);
                }

                listPokemon.Add(pokemon);
            }

            team.Id = ObjectId.GenerateNewId().ToString();
            team.Name = teamPokemon.Name;
            team.Pokemons = listPokemon;

            user.Teams.Add(team);
            try
            {
                var res = await _userManager.UpdateAsync(user);
                return res.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<InfoUserResponseDto> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return _mapper.Map<InfoUserResponseDto>(user);
        }

        public List<InfoUserResponseDto> GetUsers(string userName)
        {
            var users = _userManager.Users
                .Where(u => !string.IsNullOrEmpty(u.UserName) && u.UserName.Contains(userName))
                .ToList();

            var usersReponse = users.Select(u => _mapper.Map<InfoUserResponseDto>(u)).ToList();

            return usersReponse;
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            var res = await _userManager.UpdateAsync(user);

            return res.Succeeded;
        }

        public async Task<ApplicationUser> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return user;
        }
    }
}
