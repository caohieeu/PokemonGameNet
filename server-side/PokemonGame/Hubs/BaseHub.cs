using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PokemonGame.Core.Models.Dtos.Response;
using PokemonGame.DAL;
using PokemonGame.Exceptions;
using PokemonGame.Services.IService;

namespace PokemonGame.Hubs
{
    public class BaseHub<T> : Hub where T : class
    {
        protected readonly IUserService _userService;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly IUserContext _userContext;
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;
        public BaseHub(
            IUserService userService,
            IHttpContextAccessor contextAccessor,
            IUserContext userContext,
            IMapper mapper,
            ILogger<T> logger)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _userContext = userContext;
            _mapper = mapper;
            _logger = logger;
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
