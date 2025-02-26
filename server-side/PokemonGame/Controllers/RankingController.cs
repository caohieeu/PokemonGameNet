using Microsoft.AspNetCore.Mvc;
using PokemonGame.Models.Response;
using PokemonGame.Models;
using PokemonGame.Services;
using PokemonGame.Services.IService;

namespace PokemonGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;
        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRankings()
        {
            var response = await _rankingService.GetRankings();
            return Ok(new ApiResponse(200, "success", response));
        }
    }
}
