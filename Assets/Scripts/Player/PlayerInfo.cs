using Leveling;
using Sirenix.OdinInspector;
using ToyoSystem;
using UnityEngine;

namespace Player
{
    public class PlayerInfo : MonoBehaviour, IPlayerInfo
    {
        public float Experience;
        public int Level;
        public RANKING_TYPE Ranking;
        public int MmrLevel;
        public float MmrStreak;
        public FullToyo FullToyo;
        public float Bound;

        public bool IsWin;
        public bool IsRanked;
        public int PlayerBet;
        public int PremiumBattleTokens;
        
        public void ApplyMatchInformation(MatchInformation matchInfo)
        {
            Experience = matchInfo.Experience;
            Level = matchInfo.Level;
            Ranking = matchInfo.Ranking;
            MmrLevel = matchInfo.MmrLevel;
            MmrStreak = matchInfo.MmrStreak;
            FullToyo = matchInfo.FullToyo;
            Bound = matchInfo.Bound;
            PremiumBattleTokens = matchInfo.PremiumBattleTokens;
        }
        
        [Button]
        public void TestBattle()
        {
            MatchInformation matchInformation = new MatchInformation(this);
            MatchResults.CalculateMatchResults(ref matchInformation);
            ApplyMatchInformation(matchInformation);
        }
    }

    public struct MatchInformation
    {
        public float Experience;
        public int Level;
        public RANKING_TYPE Ranking;
        public int MmrLevel;
        public float MmrStreak;
        public FullToyo FullToyo;
        public float Bound;

        public bool isWin;
        public bool isRanked;
        public int playerBet;
        public int PremiumBattleTokens;
        public IToyoPart ToyoPart;

        public MatchInformation(PlayerInfo playerInfo)
        {
            this.Experience = playerInfo.Experience;
            Level = playerInfo.Level;
            Ranking = playerInfo.Ranking;
            MmrLevel = playerInfo.MmrLevel;
            MmrStreak = playerInfo.MmrStreak;
            FullToyo = playerInfo.FullToyo;
            Bound = playerInfo.Bound;
            isWin = playerInfo.IsWin;
            isRanked = playerInfo.IsRanked;
            playerBet = playerInfo.PlayerBet;
            PremiumBattleTokens = playerInfo.PremiumBattleTokens;
            ToyoPart = null;
        }
    }
}