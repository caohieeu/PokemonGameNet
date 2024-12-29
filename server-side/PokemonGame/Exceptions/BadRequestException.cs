using System.Runtime.CompilerServices;

namespace PokemonGame.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
