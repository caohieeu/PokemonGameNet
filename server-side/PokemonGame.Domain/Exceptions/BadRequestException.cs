using System.Runtime.CompilerServices;

namespace PokemonGame.Domain.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
