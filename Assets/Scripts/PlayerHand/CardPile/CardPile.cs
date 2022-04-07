using System;
using System.Collections.Generic;
using UnityEngine;

namespace Card.CardPile
{
    public abstract class CardPile : MonoBehaviour, ICardPile
    {
        public List<ICard> Cards { get; private set; }

        protected virtual void Awake()
        {
            Cards = new List<ICard>();
            Clear();
        }

        Action<ICard[]> ICardPile.OnPileChanged
        {
            get => OnPileChanged;
            set => OnPileChanged = value;
        }

        public virtual void AddCard(ICard card)
        {
            card.ValidateCard();

            Cards.Add(card);
            card.transform.SetParent(transform);
            NotifyPileChange();

            card.Draw();
        }

        public virtual void RemoveCard(ICard card)
        {
            card.ValidateCard();

            Cards.Remove(card);

            NotifyPileChange();
        }

        private event Action<ICard[]> OnPileChanged = hand => { };

        protected virtual void Clear()
        {
            var childCards = GetComponentsInChildren<ICard>();
            foreach (var uiCardHand in childCards)
                Destroy(uiCardHand.gameObject);

            Cards.Clear();
        }

        public void NotifyPileChange()
        {
            OnPileChanged?.Invoke(Cards.ToArray());
        }
    }
}