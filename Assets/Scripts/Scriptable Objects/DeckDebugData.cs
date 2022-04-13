    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "DeckDebugData", menuName = "ScriptableObject/DeckDebugData", order = 3)]
    public class DeckDebugData : ScriptableObject
    {
        [SerializeField]
        public List<CardData> Deck;
    }
