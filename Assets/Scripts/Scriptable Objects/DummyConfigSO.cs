using Tools;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "DummyConfig", menuName = "ScriptableObject/DummyConfig")]
    public class DummyConfigSO : ScriptableObject
    {
        [Header("Default Dummy Configuration")]
        [Header("Player")]
        public bool hpRegen;
        [ShowIf("hpRegen")] [Range(0, 5f)]
        public float hpRegenSpeed;
        public bool ignoreApCosts;
        [ShowIf("ignoreApCosts", false)] [Range(0, 5f)]
        public float apRegenSpeed;
        [Header("Enemy")]
        /*public bool enemyIA;
        [ShowIf("enemyIA")] [Range(0, 5)]
        public int enemyDifficulty;*/
        public bool enemyHpRegen;
        [ShowIf("enemyHpRegen")] [Range(0, 5f)]
        public float enemyHpRegenSpeed;
        /*[Range(0, 5f)]
        public float enemyApRegenSpeed;*/
    }
}
