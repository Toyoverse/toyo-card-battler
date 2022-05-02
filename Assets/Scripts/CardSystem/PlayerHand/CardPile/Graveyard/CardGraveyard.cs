using System;
using PlayerHand;
using UnityEngine;

namespace Card.CardPile.Graveyard
{
    public class CardGraveyard : CardPile
    {
        private Transform graveyardPosition => GlobalConfig.Instance.graveyardPosition;

        private IPlayerHand PlayerHand { get; set; }

        private void Start()
        {
            PlayerHand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>();
            PlayerHand.OnCardPlayed += AddCard;
        }

        public override void AddCard(ICard card)
        {
            card.ValidateCard();

            Cards.Add(card);
            card.transform.SetParent(graveyardPosition);
            card.gameObject.SetActive(false);
            card.Discard();
            NotifyPileChange();
        }

        public override void RemoveCard(ICard card)
        {
            card.ValidateCard();

            Cards.Remove(card);
            NotifyPileChange();
        }
    }
}