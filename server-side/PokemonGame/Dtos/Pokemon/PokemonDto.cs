using PokemonGame.Models.SubModel;

namespace PokemonGame.Dtos.Pokemon
{
    public class PokemonDto
    {
        public int PokemonId { get; set; }
        public Stat Stat { get; set; }
        public List<int> MoveIds { get; set; }
    }
}
