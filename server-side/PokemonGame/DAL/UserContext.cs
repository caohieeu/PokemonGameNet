using AutoMapper;
using MongoDB.Driver;
using PokemonGame.Dtos;
using PokemonGame.Dtos.Response;
using PokemonGame.Exceptions;
using PokemonGame.Models;
using System.IdentityModel.Tokens.Jwt;

namespace PokemonGame.DAL
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        private readonly IMongoCollection<ApplicationUser> _collection;
        private readonly IMapper _mapper;
        public UserContext(
            IMongoContext context, 
            IHttpContextAccessor contextAccessor,
            IMapper mapper)
        {
            _contextAccessor = contextAccessor;
            _database = context.Database;
            _client = context.Client;
            _mapper = mapper;
            _collection = _database.GetCollection<ApplicationUser>("Users");
        }
        public Dictionary<string, string> GetTokenInfo(string token)
        {
            try
            {
                var tokenInfo = new Dictionary<string, string>();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims.ToList();

                foreach (var claim in claims)
                {
                    tokenInfo.Add(claim.Type, claim.Value);
                }

                return tokenInfo;
            }
            catch
            {
                return null;
            }
        }
        public bool CheckToken(string token)
        {
            var tokenInfo = GetTokenInfo(token);
            string expiredDate, username;

            InfoUserResponseDto response = new InfoUserResponseDto();

            if (tokenInfo == null || !tokenInfo.TryGetValue("exp", out expiredDate))
            {
                throw new BadRequestException("Invalid token");
            }

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiredDate));
            DateTime dateTime = dateTimeOffset.UtcDateTime;
            if (dateTime <= DateTime.UtcNow || !tokenInfo.TryGetValue("UserName", out username))
            {
                return false;
            }

            return true;
        }
        public async Task<InfoUserResponseDto> GetCurrentUserInfo()
        {
            var httpContext = _contextAccessor.HttpContext;

            var token = httpContext?.Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ").Last();

            if(token == null)
            {
                throw new AuthorizationException("User is not authenticated");
            }
            var tokenInfo = GetTokenInfo(token);
            string expiredDate, username;

            InfoUserResponseDto response = new InfoUserResponseDto();

            //parse exp
            if (tokenInfo == null || !tokenInfo.TryGetValue("exp", out expiredDate))
            {
                throw new BadRequestException("Token không hợp lệ");
            }

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiredDate));
            DateTime dateTime = dateTimeOffset.UtcDateTime;
            if (dateTime > DateTime.UtcNow)
            {
                //parse UserName
                if (tokenInfo.TryGetValue("UserName", out username))
                {
                    var user = await _collection.Find(user => user.UserName == username).FirstOrDefaultAsync();
                    if (user == null)
                    {
                        throw new NotFoundException("Username is not found");
                    }

                    response = _mapper.Map<InfoUserResponseDto>(user);
                }

                return response;
            }

            throw new BadRequestException("Token không hợp lệ");
        }

        public Task<string> GetFullName()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetId()
        {
            throw new NotImplementedException();
        }
    }
}
