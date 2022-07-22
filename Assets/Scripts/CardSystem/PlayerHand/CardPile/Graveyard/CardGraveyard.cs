using System;
using Extensions;
using CardSystem.PlayerHand;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Card.CardPile.Graveyard
{
    public class CardGraveyard : CardPile
    {
        private IPlayerHand _playerHand;
        public IPlayerHand PlayerHand => _playerHand;
        
        [Inject]
        public void Construct(IPlayerHand playerHand)
        {
            _playerHand = playerHand;
        }

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
            card.transform.SetParent(transform);
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