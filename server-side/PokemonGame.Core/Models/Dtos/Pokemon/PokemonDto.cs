using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Models.Dtos.Pokemon
{
    public class PokemonDto
    {
        public int PokemonId { get; set; }
        public Stat Stat { get; set; }
        public List<int> MoveIds { get; set; }
    }
}
