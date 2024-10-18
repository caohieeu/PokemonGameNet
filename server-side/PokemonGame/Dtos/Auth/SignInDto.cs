namespace PokemonGame.Dtos.Auth
{
    public class SignInDto
    {
        public string username;
        public string password;

        public SignInDto(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
