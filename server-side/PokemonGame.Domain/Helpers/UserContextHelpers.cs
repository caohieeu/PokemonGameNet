using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PokemonGame.Core.Models.Dtos.Response;
using PokemonGame.Persistence.DAL;
using PokemonGame.Core.Interfaces.Services;
using PokemonGame.Domain.Exceptions;

namespace PokemonGame.Domain.Helpers
{
    public class UserContextHelpers<T> where T : class
    {
        private readonly IUserContextService _userContext;
        private readonly IUserService _userService;
        private readonly ILogger<T> _logger;
        public UserContextHelpers(
            IUserContextService userContext, 
            IUserService userService,
            ILogger<T> logger)
        {
            _userContext = userContext;
            _userService = userService;
            _logger = logger;
        }
        public async Task<InfoUserResponseDto> GetUserFromHubContext(HubCallerContext context)
        {
            var user = new InfoUserResponseDto();

            var httpContext = context.GetHttpContext();
            var token = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "")
                       ?? httpContext?.Request.Query["access_token"];

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token not found in Authorization header.");
            }

            try
            {
                if (_userContext.CheckToken(token))
                {
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
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);

                return null;
            }
        }
    }
}
