using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "CombatConfig", menuName = "ScriptableObject/CombatConfig")]
    public class CombatConfigSO : ScriptableObject
    {
        [Header("Combo")]
        [Tooltip("Amount by which the COMBO is divided in the calculation of HEAVY ATTACK DAMAGE.")]
        [SerializeField] [Range(1, 20)] 
        public int comboSystemFactor = 5;
        
        [Tooltip("Value added to the current combo when it increases (A new card has been successfully played).")]
        [SerializeField] [Range(1, 5)] 
        public int comboSum = 1;
        [Tooltip("Value multiplied by the current combo when it ends. Example: currentCombo * 0 = 0;")]
        [SerializeField] [Range(0, 5)] 
        public int comboBreakMultiplier = 0;

        [Header("Attack Sum")]
        [Tooltip("Value added to FAST ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float fastCardSumFactor = 0.0f;
        
        [Tooltip("Value added to HEAVY ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float heavyCardSumFactor = 1.0f;

        [Tooltip("Value added to SUPER ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float superCardSumFactor = 0.0f;

        [Header("Attack Multiplier")]
        [Tooltip("Multiplier in FAST ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float fastCardMultiplierFactor = 1.0f;
        
        [Tooltip("Multiplier in HEAVY ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float heavyCardMultiplierFactor = 1.0f;

        [Tooltip("Multiplier in SUPER ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float superCardMultiplierFactor = 1.0f;
        
        [Header("Critical")]
        [Tooltip("LUCK multiplier used to generate CRITICAL HIT CHANCE value.")] 
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float criticalLuckFactor = 0.025f;

        [Tooltip("PRECISION multiplier used to generate CRITICAL HIT CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float criticalPrecisionFactor = 0.05f;

        [Tooltip("MAXIMUM value that it is possible to obtain in the CRITICAL HIT CHANCE.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float maxCriticalChance = 0.5f;
        
        [Tooltip("Multiplier applied to the HIT VARIATION when getting a CRITICAL HIT.")]
        [SerializeField] [Range(1f, 10f)] 
        public float criticalDamageModifier = 2f;
        
        [Header("Defense")]
        [Tooltip("ENEMY defense multiplier while receiving a CRITICAL HIT.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float defenseInCriticalMultiplier = 0f;
        
        [Tooltip("Multiplier applied to ENEMY RESISTANCE when getting ENEMY DEFENSE value. (In physical attacks)")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float enemyResistanceMultiplier = 0.1f;
        
        [Tooltip("Multiplier applied to ENEMY RESILIENCE when getting ENEMY DEFENSE value. (In cyber attacks)")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float enemyResilienceMultiplier = 0.1f;
        
        [Tooltip("Multiplier factor that defines the maximum damage a defense can mitigate. " +
                 "This factor is applied to the amount of damage dealt and represents the percentage " +
                 "the defense will mitigate if it is higher than damage. " +
                 "\nExample: If the defense is greater than the damage, then the defense will be " +
                 "\n(Damage * maxDefenseFactor).")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float maxDefenseFactor = 0.5f;
        
        [Tooltip("ANALYSIS multiplier used to generate BLOCK CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float analysisMultiplier = 0.11f;
        
        [Tooltip("TECHNIQUE multiplier used to generate BLOCK CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float techDefMultiplier = 0.1f;
        
        [Tooltip("SPEED multiplier used to generate DODGE CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float speedMultiplier = 0.11f;
        
        [Tooltip("AGILITY multiplier used to generate DODGE CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float agilityDefMultiplier = 0.1f;
        
        [Header("Counter Attack")]
        [Tooltip("BASE counter attack chance (BEFORE Toyo stat calculation).")]
        [SerializeField] [Range(0.0f, 100.0f)] 
        public float baseCounterChance = 5.0f;

        [Tooltip("LUCK multiplier used to generate COUNTER ATTACK CHANCE value after dodge or block success.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float counterLuckFactor = 0.1f;
        
        [Tooltip("AGILITY multiplier used to generate COUNTER ATTACK CHANCE value after dodge success.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float counterAgilityMultiplier = 0.05f;
        
        [Tooltip("TECHNIQUE multiplier used to generate COUNTER ATTACK CHANCE value after blocker success.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float counterTechMultiplier = 0.05f;

        [Tooltip("HealthUI")] 
        [SerializeField] [Range(0.0f, 1.0f)]
        public float healthUIFillSpeed = 1.0f;
    }
}