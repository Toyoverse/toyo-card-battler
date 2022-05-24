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
        public float partMultiplier;
        public float mmrStreakMultiplier;
        public int premiumTokensSum;
    }
}
