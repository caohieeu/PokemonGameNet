using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame.Core.Constants
{
    public class Charts
    {
        public static Dictionary<string, Dictionary<string, double>> TypeChart = new()
        {
            { "Normal", new Dictionary<string, double> { { "Rock", 0.5 }, { "Ghost", 0.0 }, { "Steel", 0.5 } } },
            { "Fire", new Dictionary<string, double> { { "Grass", 2.0 }, { "Water", 0.5 }, { "Rock", 0.5 }, { "Bug", 2.0 }, { "Ice", 2.0 }, { "Steel", 2.0 }, { "Fire", 0.5 }, { "Dragon", 0.5 } } },
            { "Water", new Dictionary<string, double> { { "Fire", 2.0 }, { "Water", 0.5 }, { "Grass", 0.5 }, { "Ground", 2.0 }, { "Rock", 2.0 }, { "Dragon", 0.5 } } },
            { "Electric", new Dictionary<string, double> { { "Water", 2.0 }, { "Electric", 0.5 }, { "Grass", 0.5 }, { "Ground", 0.0 }, { "Flying", 2.0 }, { "Dragon", 0.5 } } },
            { "Grass", new Dictionary<string, double> { { "Water", 2.0 }, { "Fire", 0.5 }, { "Grass", 0.5 }, { "Poison", 0.5 }, { "Ground", 2.0 }, { "Flying", 0.5 }, { "Bug", 0.5 }, { "Rock", 2.0 }, { "Dragon", 0.5 }, { "Steel", 0.5 } } },
            { "Ice", new Dictionary<string, double> { { "Water", 0.5 }, { "Fire", 0.5 }, { "Ice", 0.5 }, { "Flying", 2.0 }, { "Ground", 2.0 }, { "Grass", 2.0 }, { "Dragon", 2.0 }, { "Steel", 0.5 } } },
            { "Fighting", new Dictionary<string, double> { { "Normal", 2.0 }, { "Rock", 2.0 }, { "Steel", 2.0 }, { "Ice", 2.0 }, { "Dark", 2.0 }, { "Flying", 0.5 }, { "Poison", 0.5 }, { "Psychic", 0.5 }, { "Bug", 0.5 }, { "Fairy", 0.5 }, { "Ghost", 0.0 } } },
            { "Poison", new Dictionary<string, double> { { "Grass", 2.0 }, { "Poison", 0.5 }, { "Ground", 0.5 }, { "Rock", 0.5 }, { "Ghost", 0.5 }, { "Steel", 0.0 }, { "Fairy", 2.0 } } },
            { "Ground", new Dictionary<string, double> { { "Fire", 2.0 }, { "Electric", 2.0 }, { "Grass", 0.5 }, { "Poison", 2.0 }, { "Flying", 0.0 }, { "Bug", 0.5 }, { "Rock", 2.0 }, { "Steel", 2.0 } } },
            { "Flying", new Dictionary<string, double> { { "Electric", 0.5 }, { "Fighting", 2.0 }, { "Bug", 2.0 }, { "Grass", 2.0 }, { "Rock", 0.5 }, { "Steel", 0.5 } } },
            { "Psychic", new Dictionary<string, double> { { "Fighting", 2.0 }, { "Poison", 2.0 }, { "Psychic", 0.5 }, { "Dark", 0.0 }, { "Steel", 0.5 } } },
            { "Bug", new Dictionary<string, double> { { "Fire", 0.5 }, { "Grass", 2.0 }, { "Fighting", 0.5 }, { "Poison", 0.5 }, { "Flying", 0.5 }, { "Psychic", 2.0 }, { "Ghost", 0.5 }, { "Dark", 2.0 }, { "Steel", 0.5 }, { "Fairy", 0.5 } } },
            { "Rock", new Dictionary<string, double> { { "Fire", 2.0 }, { "Ice", 2.0 }, { "Fighting", 0.5 }, { "Ground", 0.5 }, { "Flying", 2.0 }, { "Bug", 2.0 }, { "Steel", 0.5 } } },
            { "Ghost", new Dictionary<string, double> { { "Normal", 0.0 }, { "Psychic", 2.0 }, { "Ghost", 2.0 }, { "Dark", 0.5 } } },
            { "Dragon", new Dictionary<string, double> { { "Dragon", 2.0 }, { "Steel", 0.5 }, { "Fairy", 0.0 } } },
            { "Dark", new Dictionary<string, double> { { "Fighting", 0.5 }, { "Psychic", 2.0 }, { "Ghost", 2.0 }, { "Dark", 0.5 }, { "Fairy", 0.5 } } },
            { "Steel", new Dictionary<string, double> { { "Fire", 0.5 }, { "Water", 0.5 }, { "Electric", 0.5 }, { "Ice", 2.0 }, { "Rock", 2.0 }, { "Steel", 0.5 }, { "Fairy", 2.0 } } },
            { "Fairy", new Dictionary<string, double> { { "Fighting", 2.0 }, { "Poison", 0.5 }, { "Steel", 0.5 }, { "Dragon", 2.0 }, { "Dark", 2.0 } } }
        };
    }
}
