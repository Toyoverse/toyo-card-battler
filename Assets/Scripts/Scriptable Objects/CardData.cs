using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardData", order = 0)]
public class CardData : UniqueScriptableObject
{
    #region Public Variables

    [Header("Card Description")] [SerializeField]
    string localizedName;

    public string LocalizedName
    {
        get => localizedName;
        set => localizedName = value;
    }
    
    [SerializeField]
    CARD_TYPE cardtype;
    public CARD_TYPE CardType
    {
        get => cardtype;
        set => cardtype = value;
    }
    
    [SerializeField] 
    public Image cardImage;
    public Image CardImage
    {
        get => cardImage;
        set => cardImage = value;
    }
    
    [Header("Battle Settings")] [SerializeField] [Range(0, 10)]
    int apCost;
    public int APCost
    {
        get => apCost;
        set => apCost = value;
    }
    
    [SerializeField]
    ATTACK_TYPE attacktype;
    public ATTACK_TYPE AttackType
    {
        get => attacktype;
        set => attacktype = value;
    }
    
    [SerializeField]
    Animation attackAnimation;
    public Animation AttackAnimation
    {
        get => attackAnimation;
        set => attackAnimation = value;
    }
    
    [Space]    
    public List<HitListInfo> hitListInfos; 

    #endregion
}



[Serializable]
public class HitListInfo
{
    [SerializeField][Range(0, 500)]
    public int frameToHit;
    
    [SerializeField][Range(0, 1000.0f)]
    float damage;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }
}

public enum CARD_TYPE
{
    HEAVY,
    FAST,
    DEFENSE,
    BOND,
    SUPER
}

public enum ATTACK_TYPE
{
    CYBER,
    PHYSICAL
}

public enum DEFENSE_TYPE
{
    BLOCK,
    DODGE
}

public enum ATTACK_SUB_TYPE
{
    NEUTRAL,
    PIERCING,
    SMASHING,
    SLASHING,
    MAGNETIC,
    ELECTRIC
}