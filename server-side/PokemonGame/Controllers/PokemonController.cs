using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonGame.Core.Models.Response;
using PokemonGame.Core.Interfaces.Services;
using System.Diagnostics;

namespace PokemonGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPokemon([FromQuery] int page = 1,
            [FromQuery] int pageSize = 20, [FromQuery] string? pokemonName = null)
        {
            var response = await _pokemonService.GetPokemonAsync(page, pageSize, pokemonName);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpGet("GetAllPokemon2")]
        public async Task<IActionResult> GetAllPokemon2([FromQuery] int page = 1,
            [FromQuery] int pageSize = 20, [FromQuery] string? pokemonName = null)
        {
            var sw = new Stopwatch();
            sw.Start();
            var response = await _pokemonService.GetPokemonAsync(page, pageSize, pokemonName);
            sw.Stop();
            return Ok(new
            {
                ApiRespone = new ApiResponse(200, "success", response),
                QueryTimeMs = sw.ElapsedMilliseconds
            });
        }
        [HttpGet("{pokemonId}")]
        public async Task<IActionResult> GetPokemon(int pokemonId)
        {
            var response = await _pokemonService.GetDetailPokemonAsync(pokemonId);
            return Ok(new ApiResponse(200, "success", response));
        }
        [HttpGet("GetRandomPokemons")]
        public async Task<IActionResult> GetRandomPokemons()
        {
            var response = await _pokemonService.GetRandomPokemons();
            return Ok(new ApiResponse(200, "success", response));
        }
    }
}
