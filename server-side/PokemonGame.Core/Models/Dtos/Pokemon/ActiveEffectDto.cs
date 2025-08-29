namespace PokemonGame.Core.Models.Dtos.Pokemon
{
    public class ActiveEffectDto
    {
        public string EffectType { get; set; }
        public int Value { get; set; }
        public int RemainingDuration { get; set; }
    }
}
