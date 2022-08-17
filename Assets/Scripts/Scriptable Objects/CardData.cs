using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Tools;


[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardData", order = 0)]
public class CardData : UniqueScriptableObject
{
    #region Public Variables

    [Header("Card Description")] [SerializeField]
    public string CardName;
    
    [SerializeField]
    public CARD_TYPE CardType;
    
    [SerializeField][ShowIf("CardType", CARD_TYPE.DEFENSE)]
    public DEFENSE_TYPE DefenseType;
    
    [SerializeField] 
    public Image CardImage;

    [Header("Battle Settings")] [SerializeField] [Range(0, 10)]
    public int ApCost;
    
    [SerializeField]
    public ATTACK_TYPE AttackType;
    
    [SerializeField]
    public ATTACK_SUB_TYPE AttackSubType;
    
    [SerializeField] [Range(0f, 20.0f)] [Tooltip("Duration in seconds")]
    public float CardDuration = 2f;

    [SerializeField] [Range(0f, 2.0f)] [Tooltip("Duration before the attack")]
    public float AttackBuffer = 0.2f;
    
    //[SerializeField]
    //public Animation AttackAnimation;
    
    [SerializeField]
    public bool ApplyEffect;
    
    [SerializeField] [ShowIf("ApplyEffect")]
    public string EffectName;
    
    [SerializeField] [ShowIf("ApplyEffect")]
    public EffectData[] EffectData;
    
    [Space]    
    public List<HitListInfo> HitListInfos; 

    #endregion

    internal TOYO_PIECE toyoPart;
}

[Serializable]
public class HitListInfo
{
    [SerializeField][MinMaxSlider(0,5)]
    public Vector2 SecondToHit;
    
    [SerializeField][Range(0, 1000.0f)]
    public float Damage;
}

