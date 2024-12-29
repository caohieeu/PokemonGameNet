using PokemonGame.Dtos.Response;

namespace PokemonGame.DAL
{
    public interface IUserContext
    {
        bool CheckToken(string token);
        Dictionary<string, string> GetTokenInfo(string token);
        Task<InfoUserResponseDto> GetCurrentUserInfo();
        Task<string> GetFullName();
        Task<string> GetId();
    }
}
