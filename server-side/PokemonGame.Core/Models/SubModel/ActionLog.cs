namespace PokemonGame.Core.Models.SubModel
{
    public class ActionLog
    {
        public int Turn { get; set; }
        public string Action { get; set; }
        public string PAction { get; set; }
        public string Target { get; set; }
        public string Result { get; set; }
    }
}
