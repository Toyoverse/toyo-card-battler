using PlayerHand;
using UnityEngine;

namespace Card.CardPile.Graveyard
{
    public class CardGraveyard : CardPile
    {
        [SerializeField] [Tooltip("World point where the graveyard is positioned")]
        private Transform graveyardPosition;

        private IPlayerHand PlayerHand { get; set; }

        protected override void Awake()
        {
            base.Awake();
            PlayerHand = transform.parent.GetComponentInChildren<IPlayerHand>();
            PlayerHand.OnCardPlayed += AddCard;
        }

        public override void AddCard(ICard card)
        {
            card.ValidateCard();

            Cards.Add(card);
            card.transform.SetParent(graveyardPosition);
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