namespace PokemonGame.Core.Settings
{
    public interface IDatabaseSetting
    {
        string DBConnection {  get; set; }
        string DatabaseName { get; set; }
    }
}
