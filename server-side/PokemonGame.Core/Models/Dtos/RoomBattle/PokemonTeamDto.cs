using PokemonGame.Core.Models.SubModel;
using PokemonGame.Core.Models.Dtos.Pokemon;

namespace PokemonGame.Core.Models.Dtos.RoomBattle
{
    public class PokemonTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Type { get; set; }
        public Sprites Sprites { get; set; }
        public Species Species { get; set; }
        public Stat Stat { get; set; }
        public Stat OriginalStat { get; set; }
        public List<Abilities> Abilities { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<ActiveEffectDto> ActiveEffects { get; set; } = new();
        public List<MoveStateDto> Moves { get; set; }
    }
}
