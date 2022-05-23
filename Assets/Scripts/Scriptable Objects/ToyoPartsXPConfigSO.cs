using System;
using Tools;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "ToyoPartsXPConfigSO", menuName = "ScriptableObject/ToyoPartsXPConfigSO")]
    public class ToyoPartsXPConfigSO : ScriptableObject
    {
        [SerializeField] public ToyoPartXpMilestone[] MileStones;
        
        [SerializeField] [Tooltip("BV = Base Value")]
        
        public float BaseValue;
        
        [SerializeField] public int MaxPartLevel = 100;
    }

    [Serializable]
    public class ToyoPartXpMilestone
    {
        [Tooltip("Highest level of this milestone.")]
        public int level;

        [Tooltip("Value to be multiplied by the 'BV' (base value).")]
        public float multiplier;

        [Tooltip("Limit of possible statuses in this milestone." +
                 "\n If this milestone covers more than one level, " +
                 "this value represents how much will be added to each level in the maximum limit.")]
        public int allStatsLimit;

        [Tooltip("Limit each status individually in this milestone." +
                 "\n If this milestone covers more than one level, " +
                 "this value represents how much will be added to each level in the maximum limit.")]
        public int statLimit;
    }
}