﻿using System;
using Card;
using Card.CardPile;
using Fusion;
using ToyoSystem;
using UnityEngine;
using System.Linq;

namespace PlayerHand
{
    public class PlayerHand : CardPile, IPlayerHand
    {
        private IFullToyo _fullToyo;

        IFullToyo FullToyo 
            => _fullToyo ??= GlobalConfig.Instance.battleReferences.Toyo.GetComponent<IFullToyo>();

        public void PlaySelected()
        {
            SelectedCard.ValidateCard();
            PlayCard(SelectedCard);
        }

        public void PlayCard(ICard card)
        {
            card.ValidateCard();
            if(!card.ValidateBoundEffects(FullToyo.Buffs))
            { 
                Debug.Log("An active RULE_MOD prevents type " 
                          + card.CardData.CardType + " cards from being played.");
                return;
            }
            card.ValidateCardAP(MyPlayerRef);

            SelectedCard = null;
            RemoveCard(card);
            OnCardPlayed?.Invoke(card);
            EnableCards();
            NotifyPileChange();
        }

        public void SelectCard(ICard card)
        {
            card.ValidateCard();

            SelectedCard = card;
            DisableCards();
            NotifyCardSelected();
        }

        public void UnselectCard(ICard card)
        {
            card.ValidateCard();

            SelectedCard = null;
            card.Unselect();
            NotifyPileChange();
            EnableCards();
        }

        public void Unselect() => UnselectCard(SelectedCard);

        private void DisableCards()
        {
            foreach (var otherCard in Cards)
                otherCard.Disable();
        }

        private void EnableCards()
        {
            foreach (var otherCard in Cards)
                otherCard.Enable();
        }

        private void NotifyCardSelected()
        {
            OnCardSelected?.Invoke(SelectedCard);
        }

        #region Properties

        public ICard SelectedCard { get; private set; }
        
        public PlayerRef MyPlayerRef { get; set; }

        private event Action<ICard> OnCardPlayed = card => { };

        Action<ICard> IPlayerHand.OnCardPlayed
        {
            get => OnCardPlayed;
            set => OnCardPlayed = value;
        }

        private event Action<ICard> OnCardSelected = card => { };

        Action<ICard> IPlayerHand.OnCardSelected
        {
            get => OnCardSelected;
            set => OnCardSelected = value;
        }

        #endregion
    }
}