using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using PokemonGame.Dtos.Auth;
using PokemonGame.Dtos.Request;
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
        public async Task<IEnumerable<ApplicationUser>> GetAllUser()
        {
            return await _userService.GetAllUser();
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            var responseData = await _userService.SignIn(signInDto);
            var token = responseData.token;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                Path = "/",
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
            };

            Response.Cookies.Append("auth_token", token, cookieOptions);

            return Ok(new ApiResponse(200, "success", responseData));
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromForm] SignUpDto signUpDto, IFormFile? avatar)
        {
            var responseData = await _userService.SignUp(signUpDto, avatar);
            return Ok(new ApiResponse(200, "success", responseData));
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token", new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                Path = "/",
                Domain = "localhost",
                SameSite = SameSiteMode.Lax,
            });
            return Ok(new ApiResponse(200, "success", null));
        }
        [HttpGet("GetInfoUser")]
        public async Task<IActionResult> GetInfoUser()
        {
            return Ok(new ApiResponse(200, "Success", await _userService.GetInfoUser()));
        }
        [HttpPut("AddNewTeam/{UserId}")]
        public async Task<IActionResult> AddNewTeamPokemon(
            [FromBody] List<TeamRequestDto> teamRequestDto, string UserId)
        {
            return Ok(new ApiResponse(200, "Success", await _userService.AddNewTeamPokemon(UserId, teamRequestDto)));
        }
    }
}
