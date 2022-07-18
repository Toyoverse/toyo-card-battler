using System;
using Extensions;
using PlayerHand;
using Tools.Extensions;
using UnityEngine;

namespace Card.CardPile.Graveyard
{
    public class CardGraveyard : CardPile
    {
        private Transform graveyardPosition => GlobalConfig.Instance.graveyardPosition;

        private IPlayerHand _playerHand;
        public IPlayerHand PlayerHand => _playerHand ??= this.LazyFindOfType(ref _playerHand);

        private void OnEnable()
        {
            PlayerHand.OnAddCardToQueue += AddCard;
        }

        private void OnDisable()
        {
            PlayerHand.OnAddCardToQueue -= AddCard;
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