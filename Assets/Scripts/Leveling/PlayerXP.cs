using Player;
using ServiceLocator;

namespace Leveling
{
    public static class PlayerXP
    {
        private static int consideredMilestone;

        public static void ApplyPlayerXP(float battleXP, ref MatchInformation matchInfo)
        {
            matchInfo.Experience += battleXP;
            UpdateLevelAndXp(ref matchInfo);
        }

        private static int GetConsideredMilestone(MatchInformation matchInfo)
        {
            foreach (var mileStone in Locator.GetGlobalConfig().playerXpConfigSo.MileStones)
            {
                if (mileStone.level < matchInfo.Level)
                    continue;
                return (int)(Locator.GetGlobalConfig().playerXpConfigSo.BaseValue 
                             * mileStone.multiplier);
            }

            return (int)(Locator.GetGlobalConfig().playerXpConfigSo.BaseValue 
                         * Locator.GetGlobalConfig().playerXpConfigSo.MileStones[^1].multiplier);
        }

        private static void UpdateLevelAndXp(ref MatchInformation matchInfo)
        {
            consideredMilestone = GetConsideredMilestone(matchInfo);
            while (matchInfo.Experience >= consideredMilestone)
            {
                matchInfo.Level++;
                matchInfo.Experience -= consideredMilestone;
                consideredMilestone = GetConsideredMilestone(matchInfo);
            }
        }
    }
}
