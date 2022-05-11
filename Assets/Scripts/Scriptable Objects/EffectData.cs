using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObject/EffectData", order = 2)]
public class EffectData : ScriptableObject
{
    [Header("Effect Settings")] [SerializeField]
    public EFFECT_TYPE EffectType;
    
    [SerializeField]
    public TOYO_TYPE Toyo;
    
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.CHANGE_STAT)]
    [Tooltip("If not checked, the effect is applied instantly.")]
    public bool temporary; //TODO: Apply ShowIf Effect_Type.Rule_Mod
    
    [SerializeField] [ShowIf("EffectType",EFFECT_TYPE.CHANGE_STAT)] 
    [HideIf("temporary", false)] [Range(0.0f, 10.0f)]
    public float duration; //TODO: Apply ShowIf Effect_Type.Rule_Mod
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.GAIN_HP)] [Range(0.0f, 200.0f)]
    public float HPValue;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.GAIN_AP)] [Range(0, 9)]
    public int APValue;

    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.CHANGE_STAT)]
    public TOYO_STAT statToChange;
    
    [SerializeField] 
    [Tooltip("Multiplier applied to status. \nExamples: " +
             "\n'SPEED * 1.1' = 10% more SPEED; \n'SPEED * 0.5' = 50% less SPEED.")]
    [ShowIf("EffectType", EFFECT_TYPE.CHANGE_STAT)] [Range(0.0f, 5.0f)]
    public float changeStatFactor;
    
    [Header("Next Card Effects")] [SerializeField] 
    [Tooltip("Multiplier applied to status. \nExamples: " +
             "\n'Damage * 1.1' = 10% more damage; \n'Damage * 0.5' = 50% less damage.")]
    [ShowIf("EffectType", EFFECT_TYPE.CARD_MOD_DAMAGE)] [Range(0.0f, 5.0f)]
    public float nextCardDamageFactor;
    
    [SerializeField]
    [Tooltip("Value added to AP cost. If negative, reduces AP cost.")]
    [ShowIf("EffectType", EFFECT_TYPE.CARD_MOD_COST)] [Range(-10, 10)]
    public int nextCardCostMod; 
    //TODO: Considering that this value may be less than the current cost of the card, so if the result is negative, apply 0.

    [Tooltip("Multiplier applied to damage dealt. That is, " +
             "what percentage of the damage will increase HP. " +
             "Examples: " +
             "\n  - 'Damage * 1.1' = 110% lifeSteal; " +
             "\n  - 'Damage * 0.5' = 50% lifeSteal." +
             "\n(The final result will be rounded down. Example: " +
             "\n  - 'damage = 10, lifeSteal 12% = 1.2f, rounded = 1').")]
    [ShowIf("EffectType", EFFECT_TYPE.CARD_MOD_LIFE_STEAL)]
    [Range(0.0f, 5.0f)]
    public float lifeStealFactor;

    [SerializeField]
    [Tooltip("Card types that cannot be played.")] 
    [ShowIf("EffectType", EFFECT_TYPE.RULE_MOD)]
    public CARD_TYPE[] excludedTypes;
}