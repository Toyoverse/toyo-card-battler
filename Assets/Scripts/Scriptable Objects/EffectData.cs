using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObject/EffectData", order = 2)]
public class EffectData : ScriptableObject
{
    [Header("Effect Settings")] [SerializeField]
    public EFFECT_TYPE EffectType;
    
    [SerializeField] [Tooltip("If not checked, the effect is applied instantly.")]
    public bool temporary;
    
    [SerializeField] [ShowIf("temporary")] [Range(0.0f, 10.0f)]
    public float duration;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.GAIN_HP)] [Range(0.0f, 200.0f)]
    public float HPValue;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.GAIN_AP)] [Range(0, 9)]
    public int APValue;
    
    /*[Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.REDUCE_STAT)]
    public TOYO_STAT statToReduce;
    
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.REDUCE_STAT)] [Range(0.0f, 100.0f)]
    public float percentToReduce;*/

    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.CHANGE_STAT)]
    public TOYO_STAT statToChange;
    
    [SerializeField] 
    [Tooltip("Multiplier applied to status. \nExamples: \n'HP * 1.1' = 10% more HP; \n'HP * 0.5' = 50% less HP.")]
    [ShowIf("EffectType", EFFECT_TYPE.CHANGE_STAT)] [Range(0.0f, 5.0f)]
    public float changeFactor;
}