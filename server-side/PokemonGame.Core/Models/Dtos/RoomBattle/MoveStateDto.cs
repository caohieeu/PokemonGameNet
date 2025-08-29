using PokemonGame.Core.Models.Entities;
using PokemonGame.Core.Models.SubModel;

namespace PokemonGame.Core.Models.Dtos.RoomBattle
{
    public class MoveStateDto : Moves
    {
        public MoveEffect MoveData { get; set; }
        public int OriginalPP { get; set; }
    }
}
