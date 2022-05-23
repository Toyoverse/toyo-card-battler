using Player;
using UnityEngine;

namespace Leveling
{
    public static class PlayerXP
    {
        private static int consideredMilestone;

        public static void ApplyPlayerXP(float battleXP, ref MatchInformation matchInfo)
        {
            Debug.Log("BattleXP: " + battleXP);
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
            Debug.Log("NextMilestone: " + consideredMilestone);
            while (matchInfo.Experience >= consideredMilestone)
            {
                Debug.Log("XP >= NextMilestone");
                matchInfo.Level++;
                matchInfo.Experience -= consideredMilestone;
                consideredMilestone = GetConsideredMilestone(matchInfo);
            }
            Debug.Log("Level: " + matchInfo.Level + " | XP: " + matchInfo.Experience);
        }
    }
}
