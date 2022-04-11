using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObject/EffectData", order = 2)]
public class EffectData : ScriptableObject
{
    [Header("Effect Settings")] [SerializeField]
    public EFFECT_TYPE EffectType;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.GAIN_HP)] [Range(0.0f, 200.0f)]
    public float HPValue;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.GAIN_AP)] [Range(0, 9)]
    public int APValue;
    
    [Header("Effect Values")] 
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.REDUCE_STAT)]
    public TOYO_STAT statToReduce;
    
    [SerializeField] [ShowIf("EffectType", EFFECT_TYPE.REDUCE_STAT)] [Range(0.0f, 100.0f)]
    public float percentToReduce;

    
}