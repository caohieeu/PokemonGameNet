using MongoDB.Bson;
using PokemonGame.Dtos.Auth;
using PokemonGame.Dtos.Request;
using PokemonGame.Dtos.Response;
using PokemonGame.Models;
using PokemonGame.Models.Response;

namespace PokemonGame.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUser();
        Task<ApplicationUser> GetUserByUsernameAndPassword();
        Task<bool> SignUp(SignUpDto signUpDto, IFormFile? avatar);
        Task<AuthDto> SignIn(SignInDto user);
        Task<InfoUserResponseDto> GetInfoUser();
        Task<InfoUserResponseDto> GetUserByUsername(string username);
        Task<bool> AddNewTeamPokemon(string userId, List<TeamRequestDto> teamRequestDtos);
    }
}
