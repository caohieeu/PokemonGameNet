using PokemonGame.Core.Models;
using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Dtos.Pokemon
{
    public class AddTeamPokemonDto
    {
        public string Name { get; set; }
        public List<PokemonDto> Pokemons { get; set; }
    }
}
