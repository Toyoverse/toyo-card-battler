using Player;
using Scriptable_Objects;
using ServiceLocator;

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
            for (var i = 0; i < Locator.GetGlobalConfig().rankingXpConfigSo.Leagues.Length; i++)
            {
                var league = Locator.GetGlobalConfig().rankingXpConfigSo.Leagues[i];
                if (league.leagueRank != matchInfo.Ranking)
                    continue;
                
                actualLeague = league;
                nextLeague = (league == Locator.GetGlobalConfig().rankingXpConfigSo.Leagues[^1]) 
                    ? null : Locator.GetGlobalConfig().rankingXpConfigSo.Leagues[i+1];
                break;
            }
        }

        private static void ApplyXP(float battleXp, ref MatchInformation matchInfo)
            => matchInfo.Experience += battleXp;

        private static void UpdateLeague(ref MatchInformation matchInfo)
        {
            if (nextLeague == null)
                return;
            var _xpNeeded = nextLeague.baseMultiplier * Locator.GetGlobalConfig().rankingXpConfigSo.BaseValue;
            if (matchInfo.Experience >= _xpNeeded)
                matchInfo.Ranking = nextLeague.leagueRank;
        }

        private static float GetBetResult(float playerBet)
            => playerBet * actualLeague.betRate;
    }
}
