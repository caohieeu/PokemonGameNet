using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PokemonGame.Dtos.Auth;
using PokemonGame.Models;
using PokemonGame.Models.Response;
using PokemonGame.Services.IService;
using PokemonGame.Settings;
using System.Text.Json;

namespace PokemonGame.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetAllUser")]
        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _userService.GetAllUser();
        }
        [HttpGet("SignIn")]
        public async Task<IActionResult> SignIn()
        {
            var responseData = new SignInDto("caohieeu", "caohieeu");
            return Ok(new ApiResponse(200, "success", responseData));
        }
    }
}
