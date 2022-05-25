using Player;

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
            foreach (var mileStone in GlobalConfig.Instance.playerXpConfigSo.MileStones)
            {
                if (mileStone.level < matchInfo.Level)
                    continue;
                return (int)(GlobalConfig.Instance.playerXpConfigSo.BaseValue 
                       * mileStone.multiplier);
            }

            return (int)(GlobalConfig.Instance.playerXpConfigSo.BaseValue 
                   * GlobalConfig.Instance.playerXpConfigSo.MileStones[^1].multiplier);
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
