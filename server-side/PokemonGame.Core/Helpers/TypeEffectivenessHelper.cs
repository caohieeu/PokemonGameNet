using PokemonGame.Core.Constants;

namespace PokemonGame.Core.Helpers
{
    public static class TypeEffectivenessHelper
    {
        public static double GetEffectiveness(string atkType, List<string> defTypes)
        {
            double multiplier = 1.0;


            foreach (var defType in defTypes)
            {
                if(Charts.TypeChart.ContainsKey(atkType) && Charts.TypeChart[atkType].ContainsKey(defType))
                {
                    multiplier *= Charts.TypeChart[atkType][defType];
                }
            }

            return multiplier;
        }
    }
}
