using PokemonGame.Models;

namespace PokemonGame.Dtos.RoomBattle
{
    public class ParticipantRoomBattleDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public List<Models.Pokemon> pokemons { get; set; }
        public Models.Pokemon CurrentPokemon { get; set; }
    }
}
