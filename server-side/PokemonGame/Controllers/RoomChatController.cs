using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonGame.Dtos.RoomChat;
using PokemonGame.Models.Response;
using PokemonGame.Services.IService;

namespace PokemonGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomChatController : ControllerBase
    {
        private readonly IRoomChatService _roomChatService;
        public RoomChatController(IRoomChatService roomChatService)
        {
            _roomChatService = roomChatService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoomChat()
        {
            var response = await _roomChatService.GetAllRoomChat();
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpGet("GetParticipants/{roomId}")]
        public async Task<IActionResult> GetParticipant(string roomId)
        {
            var response = await _roomChatService.GetParticipants(roomId);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpPost]
        public async Task<IActionResult> AddNewRoomChat(RoomChatCreateDto roomChatCreateDto)
        {
            var response = await _roomChatService.AddNewRoom(roomChatCreateDto);
            return Ok(new ApiResponse(200, "success", response));
        }
    }
}
