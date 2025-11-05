using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PokemonGame.Core.Interfaces.Services;

namespace PokemonGame.Hubs
{
    public class BaseHub<T> : Hub where T : class
    {
        protected readonly IUserService _userService;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly IUserContextService _userContext;
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;
        public BaseHub(
            IUserService userService,
            IHttpContextAccessor contextAccessor,
            IUserContextService userContext,
            IMapper mapper,
            ILogger<T> logger)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _userContext = userContext;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
