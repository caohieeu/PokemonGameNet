namespace PokemonGame.Core.Models.Dtos.Auth
{
    public class SignInDto
    {
        public string username { get; set; }
        public string password { get; set; }

        public SignInDto(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
