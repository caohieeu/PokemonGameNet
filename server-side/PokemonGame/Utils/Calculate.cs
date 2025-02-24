namespace PokemonGame.Utils
{
    public class Calculate
    {
        public static double calculateStab(List<string> typePkm, string typeMove)
        {
            foreach (var type in typePkm)
            {
                if (type == typeMove)
                    return 1.5;
            }

            return 1.0;
        }
        public static int CalculateDamage(int attackerLevel, long movePower, long atkStat,
            long defStat, double stab, double typeEffectiveness, bool isCritical)
        {
            var baseDamage = (((2 * attackerLevel / 5.0) + 2) * movePower * (atkStat / (double)defStat)) / 50 + 2;

            double critical = isCritical ? 1.5 : 1.0;

            Random rnd = new Random();
            double randomFactor = rnd.NextDouble() * (1.00 - 0.85) + 0.85;

            double totalDamage = baseDamage * stab * critical *typeEffectiveness * randomFactor;

            return (int)Math.Max(1, totalDamage);
        }
        public static int CalculateK(int point)
        {
            if (point <= 1600)
                return 25;
            else if (point <= 2000)
                return 20;
            else if (point <= 2400)
                return 15;
            else
                return 10;
        }
    }
}
