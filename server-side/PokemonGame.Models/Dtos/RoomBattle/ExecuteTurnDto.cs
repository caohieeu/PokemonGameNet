namespace PokemonGame.Dtos.RoomBattle
{
    public class ExecuteTurnDto
    {
        public string roomId {  get; set; }
        public int moveId { get; set; }
        public string usernamePlayer { get; set; }
        public int newPokemon {  get; set; }
        public string type { get; set; }
    }
}
