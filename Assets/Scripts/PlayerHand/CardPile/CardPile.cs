using System;
using System.Collections.Generic;
using UnityEngine;

namespace Card.CardPile
{
    public abstract class CardPile : MonoBehaviour, ICardPile
    {
        event Action<ICard[]> OnPileChanged = hand => { };

        
        Action<ICard[]> ICardPile.OnPileChanged
        {
            get => OnPileChanged;
            set => OnPileChanged = value;
        }
        
        public List<ICard> Cards { get; private set; }

        protected virtual void Awake()
        {
            //initialize register
            Cards = new List<ICard>();

            Clear();
        }
        
        public void AddCard(ICard card)
        {
            if (!CardUtils.ValidateCard(card)) return;
            
            Cards.Add(card);
            card.transform.SetParent(transform);
            NotifyPileChange();

            card.Draw();
        }

        public void RemoveCard(ICard card)
        {
            if (!CardUtils.ValidateCard(card)) return;

            Cards.Remove(card);

            NotifyPileChange();
        }
        
        protected virtual void Clear()
        {
            var childCards = GetComponentsInChildren<ICard>();
            foreach (var uiCardHand in childCards)
                Destroy(uiCardHand.gameObject);

            Cards.Clear();
        }
        
        public void NotifyPileChange() => OnPileChanged?.Invoke(Cards.ToArray());

    }
}