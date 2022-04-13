using System;
using UnityEngine;

namespace Card.CardPile.Graveyard
{
    [RequireComponent(typeof(CardGraveyard))]
    public class CardGraveyardSorter : MonoBehaviour
    {
        private Transform graveyardPosition => GlobalConfig.Instance.graveyardPosition;

        private ICardPile CardGraveyard { get; set; }

        private void Awake()
        {
            CardGraveyard = GetComponent<CardGraveyard>();
            CardGraveyard.OnPileChanged += Sort;
        }


        public void Sort(ICard[] cards)
        {
            if (cards == null)
                throw new ArgumentException("Can't sort a card list null");

            var _lastPos = cards.Length - 1;
            var _lastCard = cards[_lastPos];
            var _gravPos = graveyardPosition.position + new Vector3(0, 0, -5);
            var _backGravPos = graveyardPosition.position;

            _lastCard.MoveToWithZ(_gravPos, 4); // Todo Movement Speed

            for (var i = 0; i < cards.Length - 1; i++)
            {
                var card = cards[i];
                card.MoveToWithZ(_backGravPos, 4); // Todo Movement Speed
            }
        }
    }
}