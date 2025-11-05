using Microsoft.AspNetCore.Mvc;

namespace PokemonGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ClearCache()
        {
            // Implement cache clearing logic here
            return Ok(new { message = "Cache cleared successfully." });
        }
    }
}
