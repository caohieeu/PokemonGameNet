namespace PokemonGame.Settings
{
    public interface IDatabaseSetting
    {
        string DBConnection {  get; set; }
        string DatabaseName { get; set; }
    }
}
