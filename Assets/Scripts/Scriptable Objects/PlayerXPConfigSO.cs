using System;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PlayerXPConfigSO", menuName = "ScriptableObject/PlayerXPConfigSO")]
    public class PlayerXPConfigSO : ScriptableObject
    {
        [Header("Player Experience")][SerializeField]
        public PlayerXpMilestone[] MileStones;
        [SerializeField] [Tooltip("BV = Base Value")]
        public float BaseValue;
        [SerializeField] 
        public int MaxPlayerLevel = 100;
        [SerializeField]
        public Reward[] Rewards;
    }
}

[Serializable]
public class PlayerXpMilestone
{
    [Tooltip("Highest level of this milestone.")]
    public int level;
    [Tooltip("Value to be multiplied by the 'BV' (base value).")]
    public float multiplier;
}

[Serializable]
public class Reward
{
    public int level;
    public string reward;
}
