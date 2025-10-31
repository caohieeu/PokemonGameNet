namespace PokemonGame.Core.Settings
{
    public class DatabaseSetting : IDatabaseSetting
    {
        public string DBConnection { get; set; } = null;
        public string DatabaseName { get; set; } = null;
    }
}
