namespace PokemonGame.Settings
{
    public class DatabaseSetting : IDatabaseSetting
    {
        public string DBConnection { get; set; } = null;
        public string DatabaseName { get; set; } = null;
    }
}
