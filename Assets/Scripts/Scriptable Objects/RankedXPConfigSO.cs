using System;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "RankedXPConfigSO", menuName = "ScriptableObject/RankedXPConfigSO")]
    public class RankedXPConfigSO : ScriptableObject
    {
        public League[] Leagues;
        [SerializeField] [Tooltip("BV = Base Value")]
        public float BaseValue;
    }

    [Serializable]
    public class League
    {
        public RANKING_TYPE leagueRank;
        [Tooltip("Experience required to reach this league.")]
        public float baseMultiplier; 
        public float xpWonPerMatch;
        public float xpLostPerMatch;
        [Tooltip("Values that can be bet.")]
        public float[] betValues;
        public float betRate;
    }
}