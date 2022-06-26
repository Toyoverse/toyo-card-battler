using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObject/EffectData", order = 2)]
public class EffectData : ScriptableObject
{
    [Header("Effect Settings")] [SerializeField]
    public EFFECT_TYPE EffectType;
    
    [SerializeField]
    public TOYO_TYPE Toyo;
    
    [SerializeField]
    [Tooltip("If not checked, the effect is applied instantly.")]
    public bool temporary; //TODO: Don't show for effect types that can only be instantaneous.

    [SerializeField]
    [ShowIf("@temporary == true")]
    [Range(0.0f, 10.0f)]
    [Tooltip("[WARNING! While the turn system has not been defined] \n" +
             "This variable represents the number of times the effect will be applied. " +
             "That is, if the value was 2, this effect will be applied to two actions, " +
             "regardless of how many turns pass.")]
    public int duration; //TODO: Don't show for effect types that can only be instantaneous.

    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.HP_MOD)] [Range(-200.0f, 200.0f)]
    public float HPValue;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.AP_MOD)] [Range(-9, 9)]
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
    [Tooltip("Damage multiplication factor. " +
             "\nThe factor starts at 1, adds the buffs (this value) and then multiplies by the damage. " +
             "\nThat is, if this value of is 0.5, the end result is (Damage * 1.5), " +
             "increasing 50% of the total damage. \nIf it is a negative value, " +
             "it will reduce the damage, for example: If this value is -0.2, " +
             "the end result will be (Damage * 0.8), reduced by 20% of the total damage.")]
    [ShowIf("EffectType", EFFECT_TYPE.CARD_MOD_DAMAGE)] [Range(-1.0f, 5.0f)]
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
             "\n  - 'Damage * 0.5' = 50% lifeSteal.")]
    [ShowIf("EffectType", EFFECT_TYPE.CARD_MOD_LIFE_STEAL)]
    [Range(0.0f, 5.0f)]
    public float lifeStealFactor;

    [SerializeField]
    [Tooltip("Card types that cannot be played.")] 
    [ShowIf("EffectType", EFFECT_TYPE.RULE_MOD)]
    public CARD_TYPE[] excludedTypes;
}