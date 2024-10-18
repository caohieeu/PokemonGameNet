using PokemonGame.Dtos.Auth;
using PokemonGame.Models;

namespace PokemonGame.Services.IService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User> GetUserByUsernameAndPassword();
        Task<bool> SignUp();
        Task<SignInDto> SignIn();
    }
}
