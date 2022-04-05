using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CARD_TYPE
{
    HEAVY,
    FAST,
    BLOCK,
    SPECIAL
}

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObject/CardData", order = 1)]
public class CardData : ScriptableObject
{
    #region Internals
    
    internal int id;
    
    #endregion

    #region Public Variables
    
    public string localizedName; 
    public float damage;
    public CARD_TYPE type;
    public Image cardImage;
    
    #endregion

    
}
