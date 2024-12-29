using PokemonGame.Models;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Dtos.Request
{
    public class TeamRequestDto
    {
        public int PokemonId { get; set; }
        public Stat Stat { get; set; }
        public List<int> MoveIds { get; set; }
        public int TotalStat { get; set; } = 508;
    }
}
