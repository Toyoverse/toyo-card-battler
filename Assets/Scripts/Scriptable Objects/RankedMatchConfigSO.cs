using System;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "RankedMatchConfigSO", menuName = "ScriptableObject/RankedMatchConfigSO")]
    public class RankedMatchConfigSO : ScriptableObject
    {
        [Tooltip("Premium tokens needed to earn BOUND.")]
        public int tokens4Bound;

        [Header("Winner Scenario")] 
        public RankedMatchInformation winConfig;

        [Header("Loser Scenario")]
        public RankedMatchInformation loseConfig;
    }

    [Serializable]
    public class RankedMatchInformation
    {
        public float playerXPSum;
        public float playerXpMmrMultiplier;
        [Tooltip("If it's a win, consider a multiplication factor, if it's a defeat, consider a divisor.")]
        public float mmrStreakFactor;
        public float partDivider;
        public float rankXpMmrDivider;
    }
}
