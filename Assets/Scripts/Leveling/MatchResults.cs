using System;
using System.Linq;
using Player;
using Scriptable_Objects;
using ServiceLocator;
using ToyoSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Leveling
{
    public static class MatchResults 
    {
        public static void CalculateMatchResults(ref MatchInformation matchInfo)
        {
            if (matchInfo.isWin)
                CalculateWinResult(ref matchInfo);
            else
                CalculateLoseResult(ref matchInfo);
        }

        private static void CalculateWinResult(ref MatchInformation matchInfo)
        {
            var _oldLevel = matchInfo.Level;
            PlayerXP.ApplyPlayerXP(GetPlayerXpInWin(matchInfo), ref matchInfo);
            matchInfo.ToyoPart = GetRandomPart(matchInfo.FullToyo);
            ToyoPartsXP.ApplyPartXP(GetPartXpInWin(matchInfo), ref matchInfo);
            matchInfo.MmrStreak = GetMmrStreakInWin(matchInfo);
            if (matchInfo.isRanked)
            {
                RankingXP.ApplyRankingXP(GetRankedXpInWin(matchInfo), ref matchInfo);
                matchInfo.Bound -= matchInfo.playerBet;
                matchInfo.Bound = GetBoundInWin(matchInfo);
            }
            else
            {
                matchInfo.PremiumBattleTokens = GetPremiumTokensInWin(ref matchInfo);
                CheckUnlockRewards(matchInfo, _oldLevel);
            }
        }
        
        private static void CalculateLoseResult(ref MatchInformation matchInfo)
        {
            PlayerXP.ApplyPlayerXP(GetPlayerXpInLose(matchInfo), ref matchInfo);
            matchInfo.ToyoPart = GetRandomPart(matchInfo.FullToyo);
            ToyoPartsXP.ApplyPartXP(GetPartXpInLose(matchInfo), ref matchInfo);
            matchInfo.MmrStreak = GetMmrStreakInLose(matchInfo);
            if (matchInfo.isRanked)
            {
                matchInfo.Bound -= matchInfo.playerBet;
                RankingXP.ApplyRankingXP(GetRankedXpInLose(matchInfo), ref matchInfo);
            }
            else
                matchInfo.PremiumBattleTokens = GetPremiumTokensInLose(matchInfo);
        }

        private static IToyoPart GetRandomPart(FullToyo fullToyo)
            => fullToyo.ToyoParts[(TOYO_PIECE)Random.Range(0, 
                Enum.GetValues(typeof(TOYO_PIECE)).Cast<int>().Max())];
        //TODO: Receber parte do globalconfig

        private static float GetPlayerXpInWin(MatchInformation matchInfo)
            => matchInfo.isRanked
                ? Locator.GetGlobalConfig().rankedMatchConfigSo.winConfig.playerXPSum +
                  (matchInfo.MmrLevel * Locator.GetGlobalConfig().rankedMatchConfigSo.winConfig.playerXpMmrMultiplier)
                : matchInfo.MmrLevel + Locator.GetGlobalConfig().normalMatchConfigSo.winConfig.playerXPSum;

        private static float GetPlayerXpInLose(MatchInformation matchInfo)
            => matchInfo.isRanked
                ? Locator.GetGlobalConfig().rankedMatchConfigSo.loseConfig.playerXPSum +
                  (matchInfo.MmrLevel * Locator.GetGlobalConfig().rankedMatchConfigSo.loseConfig.playerXpMmrMultiplier)
                : matchInfo.MmrLevel + Locator.GetGlobalConfig().normalMatchConfigSo.loseConfig.playerXPSum;

        private static float GetPartXpInWin(MatchInformation matchInfo)
            => matchInfo.isRanked
                ? matchInfo.MmrLevel / Locator.GetGlobalConfig().rankedMatchConfigSo.winConfig.partDivider
                : matchInfo.MmrLevel * Locator.GetGlobalConfig().normalMatchConfigSo.winConfig.partMultiplier;

        private static float GetPartXpInLose(MatchInformation matchInfo)
            => matchInfo.isRanked
                ? matchInfo.MmrLevel / Locator.GetGlobalConfig().rankedMatchConfigSo.loseConfig.partDivider
                : (float)Math.Ceiling(matchInfo.MmrLevel * Locator.GetGlobalConfig().normalMatchConfigSo.loseConfig.partMultiplier);

        private static float GetRankedXpInWin(MatchInformation matchInfo)
            => GetActiveLeague(matchInfo).xpWonPerMatch +
                   (float)Math.Floor(matchInfo.MmrLevel / Locator.GetGlobalConfig().rankedMatchConfigSo.winConfig.rankXpMmrDivider);

        private static float GetRankedXpInLose(MatchInformation matchInfo)
            => - GetActiveLeague(matchInfo).xpLostPerMatch +
               (float)Math.Floor(matchInfo.MmrLevel / Locator.GetGlobalConfig().rankedMatchConfigSo.loseConfig.rankXpMmrDivider);

        private static float GetBoundInWin(MatchInformation matchInfo)
            => matchInfo.playerBet * GetActiveLeague(matchInfo).betRate;

        private static float GetMmrStreakInWin(MatchInformation matchInfo)
            => matchInfo.isRanked
                ? matchInfo.MmrStreak * Locator.GetGlobalConfig().rankedMatchConfigSo.winConfig.mmrStreakMultiplier
                : matchInfo.MmrStreak * Locator.GetGlobalConfig().normalMatchConfigSo.winConfig.mmrStreakMultiplier;

        private static float GetMmrStreakInLose(MatchInformation matchInfo)
            => matchInfo.isRanked
                ? matchInfo.MmrStreak * Locator.GetGlobalConfig().rankedMatchConfigSo.loseConfig.mmrStreakMultiplier
                : matchInfo.MmrStreak * Locator.GetGlobalConfig().normalMatchConfigSo.loseConfig.mmrStreakMultiplier;
        
        private static League GetActiveLeague(MatchInformation matchInfo)
            => Locator.GetGlobalConfig().rankingXpConfigSo.Leagues.FirstOrDefault(league => league.leagueRank == matchInfo.Ranking);

        private static int GetPremiumTokensInWin(ref MatchInformation matchInfo)
        {
            var _tokens4Bound = Locator.GetGlobalConfig().normalMatchConfigSo.premiumTokens4Bound;
            if (matchInfo.PremiumBattleTokens >= _tokens4Bound)
            {
                matchInfo.Bound++;
                return matchInfo.PremiumBattleTokens - _tokens4Bound;
            }
            else 
                return matchInfo.PremiumBattleTokens + Locator.GetGlobalConfig().normalMatchConfigSo.winConfig.premiumTokensSum;
        }

        private static int GetPremiumTokensInLose(MatchInformation matchInfo)
            => matchInfo.PremiumBattleTokens - Locator.GetGlobalConfig().normalMatchConfigSo.loseConfig.premiumTokensSum > 0
                    ? matchInfo.PremiumBattleTokens - Locator.GetGlobalConfig().normalMatchConfigSo.loseConfig.premiumTokensSum
                    : 0;

        private static void CheckUnlockRewards(MatchInformation matchInfo, int oldLevel)
        {
            foreach (var reward in Locator.GetGlobalConfig().playerXpConfigSo.Rewards)
            {
                if(reward.level <= oldLevel)
                    continue;
                if (matchInfo.Level >= reward.level)
                    Debug.Log("Reward: " + reward.reward);
            }
        }
    }
}
