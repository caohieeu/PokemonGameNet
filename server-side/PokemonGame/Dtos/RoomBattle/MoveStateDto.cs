using PokemonGame.Models;
using PokemonGame.Models.SubModel;

namespace PokemonGame.Dtos.RoomBattle
{
    public class MoveStateDto : Moves
    {
        public MoveEffect MoveData { get; set; }
        public int OriginalPP { get; set; }
    }
}
