using System;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "NormalMatchConfigSO", menuName = "ScriptableObject/NormalMatchConfigSO")]
    public class NormalMatchConfigSO : ScriptableObject
    {
        [Tooltip("Premium tokens needed to earn BOUND.")]
        public int premiumTokens4Bound;

        [Header("Winner Scenario")] 
        public NormalMatchInformation winConfig;

        [Header("Loser Scenario")]
        public NormalMatchInformation loseConfig;
    }

    [Serializable]
    public class NormalMatchInformation
    {
        public float playerXPSum;
        [Tooltip("If it's a win, consider a multiplication factor, if it's a defeat, consider a divisor.")]
        public float partFactor;
        [Tooltip("If it's a win, consider a multiplication factor, if it's a defeat, consider a divisor.")]
        public float mmrStreakFactor;
        public int premiumTokensSum;
    }
}
