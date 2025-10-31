using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonGame.Core.Models.Dtos.RoomBattle;
using PokemonGame.Core.Models.Response;
using PokemonGame.Core.Interfaces.Services;

namespace PokemonGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBattleController : ControllerBase
    {
        private readonly IRoomBattleService _roomBattleService;
        public RoomBattleController(IRoomBattleService roomBattleService)
        {
            _roomBattleService = roomBattleService;
        }
        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomBattle(string roomId)
        {
            var response = await _roomBattleService.GetRoomBattle(roomId);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpPost]
        public async Task<IActionResult> AddRoomBattle([FromBody] AddRoomBattleDto addRoomBattleDto)
        {
            var response = await _roomBattleService.AddRoomBattle();
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpDelete("{roomId}")]
        public async Task<IActionResult> RemoveRoomBattle(string roomId)
        {
            var response = await _roomBattleService.RemoveRoomBattle(roomId);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpGet("GetParicipant")]
        public async Task<IActionResult> GetParicipant([FromQuery] GetParticipantDto getParticipantDto)
        {
            var response = await _roomBattleService.GetParticipant(getParticipantDto.RoomId, getParticipantDto.UserName);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpPost("AddParicipant")]
        public async Task<IActionResult> AddParicipant([FromBody] RandomPokemonDto randomPokemonDto)
        {
            var response = await _roomBattleService.AddParticipant(randomPokemonDto);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpPost("SwitchPokemon")]
        public async Task<IActionResult> SwitchPokemon([FromBody] SwitchPokemonDto switchPokemon)
        {
            var response = await _roomBattleService.SwitchPokemon(switchPokemon);
            return Ok(new ApiResponse(200, "success", response));
        }
    }
}
