using System;
using System.Collections.Generic;
using Card;
using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "ToyoPart", menuName = "ScriptableObject/ToyoPart")]
[UseAttributes]
public class ToyoPartSO : ScriptableObject
{
    [Header("ToyoPiece")]
    [Space]
    public TOYO_PIECE ToyoPiece;

    public ToyoIDData ToyoId;
    
    [Header("Technoallow")]
    [Space]
    public TOYO_TECHNOALLOY ToyoTechnoalloy;
    
    [Space]
    public List<ToyoPartStat> ToyoPartStat;
    
    [Space]
    public List<ToyoPartStat> ToyoBonusStat;
    
    [Space]
    public List<CardData> CardsFromPiece;
}

[Serializable]
public class ToyoPartStat
{
    public TOYO_STAT ToyoStat;
    public float StatValue;
}

