using System.Collections.Generic;
using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckDatabaseSO", menuName = "ScriptableObject/DeckDatabaseSO")]
[UseAttributes]
public class DeckDatabaseSO : ScriptableObject
{
    public List<CardData> AllCards;
}