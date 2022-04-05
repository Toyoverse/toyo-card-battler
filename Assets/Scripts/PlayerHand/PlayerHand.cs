using System;
using System.Collections.Generic;
using Card;
using Card.CardPile;
using UnityEngine;

namespace PlayerHand
{
    public class PlayerHand : CardPile, IPlayerHand
    {

        #region Properties

        public ICard SelectedCard { get; private set; }

        event Action<ICard> OnCardPlayed = card => { };
        
        Action<ICard> IPlayerHand.OnCardPlayed
        {
            get => OnCardPlayed;
            set => OnCardPlayed = value;
        }
        
        event Action<ICard> OnCardSelected = card => { };
        
        Action<ICard> IPlayerHand.OnCardSelected
        { 
            get => OnCardSelected;
            set => OnCardSelected = value;
        }

        #endregion
        
        
        public void PlaySelected()
        {
            if (!CardUtils.ValidateCard(SelectedCard)) return;
            PlayCard(SelectedCard);
        }

        public void PlayCard(ICard card)
        {
            if (!CardUtils.ValidateCard(card)) return;

            SelectedCard = null;
            RemoveCard(card);
            OnCardPlayed?.Invoke(card);
            EnableCards();
            NotifyPileChange();
        }

        public void SelectCard(ICard card)
        {
            if (!CardUtils.ValidateCard(card)) return;
            
            SelectedCard = card;
            DisableCards();
            NotifyCardSelected();
        }

        public void UnselectCard(ICard card)
        {
            if (!CardUtils.ValidateCard(card)) return;

            SelectedCard = null;
            card.Unselect();
            NotifyPileChange();
            EnableCards();
        }
        
        public void Unselect() => UnselectCard(SelectedCard);
        
        public void DisableCards()
        {
            foreach (var otherCard in Cards)
                otherCard.Disable();
        }

        public void EnableCards()
        {
            foreach (var otherCard in Cards)
                otherCard.Enable();
        }
        
        void NotifyCardSelected() => OnCardSelected?.Invoke(SelectedCard);

    }
}