using PokemonGame.Core.Models;

namespace PokemonGame.Core.Models.Dtos.RoomBattle
{
    public class ParticipantRoomBattleDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public List<PokemonTeamDto> pokemons { get; set; }
        public PokemonTeamDto CurrentPokemon { get; set; }
    }
}
