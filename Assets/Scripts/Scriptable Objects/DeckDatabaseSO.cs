using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckDatabaseSO", menuName = "ScriptableObject/DeckDatabaseSO")]
public class DeckDatabaseSO : ScriptableObject
{
    public List<CardData> AllCards;
}