using System.Collections;
using System.Linq;
using Player;
using Scriptable_Objects;
using ToyoSystem;
using UnityEngine;

namespace Leveling
{
    public static class ToyoPartsXP 
    {
        private static ToyoPartXpMilestone consideredMilestone;
        private static float milestoneBaseValue;
        
        public static void ApplyPartXP(float battleXP, ref MatchInformation matchInfo)
        {
            matchInfo.ToyoPart.Experience += battleXP;
            UpdateStatsAndXp(ref matchInfo);
        }

        private static ToyoPartXpMilestone GetConsideredMilestone(MatchInformation matchInfo)
        {
            foreach (var mileStone in GlobalConfig.Instance.partXpConfigSo.MileStones)
            {
                if (mileStone.level < matchInfo.ToyoPart.Level)
                    continue;
                return mileStone;
            }

            return GlobalConfig.Instance.partXpConfigSo.MileStones[^1];
        }

        private static void UpdateStatsAndXp(ref MatchInformation matchInfo)
        {
            consideredMilestone = GetConsideredMilestone(matchInfo);
            if (GetPlayerAllStats(matchInfo) >= consideredMilestone.allStatsLimit)
            {
                Debug.Log("This part is already at the maximum allowed status, evolve and try again.");
                return;
            }

            milestoneBaseValue = GlobalConfig.Instance.partXpConfigSo.BaseValue;
            var mileStoneValue = milestoneBaseValue * consideredMilestone.multiplier;
            if (matchInfo.ToyoPart.Experience >= mileStoneValue)
            {
                matchInfo.ToyoPart.Experience -= mileStoneValue;
                GoChoseStat(matchInfo);
            }
        }

        private static void GoChoseStat(MatchInformation matchInfo)
        {
            //TODO: UX - Choose the status to evolve.
        }

        //TODO: This part can be called externally by a button after the status is selected.
        private static void UpdateStat(ref int statValue, ref MatchInformation matchInfo)
        {
            if (statValue >= consideredMilestone.statLimit)
            {
                Debug.Log("This status is already at the limit, try another one.");
                return;
            }

            statValue++;
            
            consideredMilestone = GetConsideredMilestone(matchInfo);
            var mileStoneValue = milestoneBaseValue * consideredMilestone.multiplier;
            if (matchInfo.ToyoPart.Experience >= mileStoneValue)
                UpdateStatsAndXp(ref matchInfo);
        }

        private static void UpdateLevel(ref MatchInformation matchInfo)
        {
            //TODO: Call this via a button on the workshop interface.
            if (matchInfo.ToyoPart.Level < GlobalConfig.Instance.partXpConfigSo.MaxPartLevel)
                matchInfo.ToyoPart.Level++;
            else
                Debug.Log("This piece is at maximum level, try another one.");
        }

        private static float GetPlayerAllStats(MatchInformation matchInfo)
            => matchInfo.ToyoPart.PartStat.Sum(stat => stat.StatValue);
    }
}
