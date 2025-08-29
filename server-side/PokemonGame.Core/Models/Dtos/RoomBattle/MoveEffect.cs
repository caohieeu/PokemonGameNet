namespace PokemonGame.Core.Models.Dtos.RoomBattle
{
    public class MoveEffect
    {
        public string Effect { get; set; }
        public string Target { get; set; }
        public int EffectValue { get; set; }
        public double EffectChance { get; set; }
        public int Duration { get; set; }
    }
}
