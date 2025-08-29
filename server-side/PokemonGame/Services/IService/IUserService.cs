using MongoDB.Bson;
using PokemonGame.Core.Models.Dtos.Auth;
using PokemonGame.Core.Models.Dtos.Pokemon;
using PokemonGame.Core.Models.Dtos.Response;
using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.Response;

namespace PokemonGame.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUser();
        Task<ApplicationUser> GetUser(string username);
        Task<ApplicationUser> GetUserByUsernameAndPassword();
        Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar);
        Task<AuthDto> SignIn(SignInDto user);
        Task<InfoUserResponseDto> GetInfoUser();
        Task<bool> UpdateUser(ApplicationUser user);
        Task<InfoUserResponseDto> GetUserByUsername(string username);
        Task<bool> AddNewTeamPokemon(string userId, AddTeamPokemonDto teamPokemon);
        List<InfoUserResponseDto> GetUsers(string userName);
    }
}
