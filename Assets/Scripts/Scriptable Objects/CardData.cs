using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardData", order = 0)]
public class CardData : UniqueScriptableObject
{
    #region Public Variables

    [Header("Card Description")] [SerializeField]
    public string LocalizedName;
    
    [SerializeField]
    public CARD_TYPE Cardtype;
    
    [SerializeField] 
    public Image CardImage;

    [Header("Battle Settings")] [SerializeField] [Range(0, 10)]
    public int ApCost;
    
    [SerializeField]
    public ATTACK_TYPE AttackType;
    
    [SerializeField]
    public ATTACK_SUB_TYPE AttackSubType;
    
    [SerializeField]
    public Animation AttackAnimation;
    
    [SerializeField]
    public bool ApplyEffect;
    
    [SerializeField] [ShowIf("ApplyEffect")]
    public EffectData EffectData;
    
    [Space]    
    public List<HitListInfo> HitListInfos; 

    #endregion
}

[Serializable]
public class HitListInfo
{
    [SerializeField][Range(0, 500)]
    public int FrameToHit;
    
    [SerializeField][Range(0, 1000.0f)]
    public float Damage;
}

