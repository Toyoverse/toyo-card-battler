using System;
using Card;
using Card.CardPile;

namespace PlayerHand
{
    public class PlayerHand : CardPile, IPlayerHand
    {
        public void PlaySelected()
        {
            SelectedCard.ValidateCard();
            PlayCard(SelectedCard);
        }

        public void PlayCard(ICard card)
        {
            card.ValidateCard();
            card.ValidateCardAP();

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

        public void Unselect()
        {
            UnselectCard(SelectedCard);
        }

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

        private void NotifyCardSelected()
        {
            OnCardSelected?.Invoke(SelectedCard);
        }

        #region Properties

        public ICard SelectedCard { get; private set; }

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