using Player;
using Scriptable_Objects;
using UnityEngine.Networking.Match;

namespace Leveling
{
    public static class RankingXP
    {
        private static League actualLeague;
        private static League nextLeague;

        public static void ApplyRankingXP(float battleXp, ref MatchInformation matchInfo)
        {
            SetActualAndNextLeague(matchInfo);
            matchInfo.Bound += GetBetResult(matchInfo.playerBet);
            ApplyXP(battleXp, ref matchInfo);
            UpdateLeague(ref matchInfo);
        }

        private static void SetActualAndNextLeague(MatchInformation matchInfo)
        {
            for (var i = 0; i < GlobalConfig.Instance.rankingXpConfigSo.Leagues.Length; i++)
            {
                var league = GlobalConfig.Instance.rankingXpConfigSo.Leagues[i];
                if (league.leagueRank != matchInfo.Ranking)
                    continue;
                
                actualLeague = league;
                nextLeague = (league == GlobalConfig.Instance.rankingXpConfigSo.Leagues[^1]) 
                    ? null : GlobalConfig.Instance.rankingXpConfigSo.Leagues[i+1];
                break;
            }
        }

        private static void ApplyXP(float battleXp, ref MatchInformation matchInfo)
            => matchInfo.Experience += battleXp;

        private static void UpdateLeague(ref MatchInformation matchInfo)
        {
            if (nextLeague == null)
                return;
            var _xpNeeded = nextLeague.baseMultiplier * GlobalConfig.Instance.rankingXpConfigSo.BaseValue;
            if (matchInfo.Experience >= _xpNeeded)
                matchInfo.Ranking = nextLeague.leagueRank;
        }

        private static float GetBetResult(float playerBet)
            => playerBet * actualLeague.betRate;
    }
}
