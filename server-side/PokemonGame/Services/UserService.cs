using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PokemonGame.Dtos.Auth;
using PokemonGame.Models;
using PokemonGame.Repositories.IRepository;
using PokemonGame.Services.IService;
using PokemonGame.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace PokemonGame.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSetting _appSetting;
        public UserService(IUserRepository userRepository, 
            IOptionsMonitor<AppSetting> options)
        {
            _userRepository = userRepository;
            _appSetting = options.CurrentValue;
        }
        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _userRepository.GetAll(Builders<User>.Filter.Empty);
        }

        public Task<SignInDto> SignIn()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SignUp()
        {
            _
        }
        public string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserName", user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_appSetting.SecretKey),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
