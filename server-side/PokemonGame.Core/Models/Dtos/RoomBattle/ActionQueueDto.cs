namespace PokemonGame.Core.Models.Dtos.RoomBattle
{
    public class ActionQueueDto
    {
        public string Player { get; set; }
        public string ActionType { get; set; } //Attack or Switch
        public int MoveId { get; set; }
        public int NewPokemonId { get; set; }
        public int Speed { get; set; }
        public int Priority { get; set; }
    }
}
