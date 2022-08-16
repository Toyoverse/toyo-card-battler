using System;
using Extensions;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Card.CardPile.Graveyard
{
    [RequireComponent(typeof(CardGraveyard))]
    public class CardGraveyardSorter : MonoBehaviour
    {
        private Transform _graveyardPosition;
        
        private ICardPile _cardGraveyard;
        public ICardPile CardGraveyard => _cardGraveyard;
        
        [Inject]
        public void Construct(CardGraveyard graveyard)
        {
            _graveyardPosition = graveyard.transform;
            _cardGraveyard = graveyard;
        }

        private void OnEnable()
        {
            CardGraveyard.OnPileChanged += Sort;
        }

        private void OnDisable()
        {
            CardGraveyard.OnPileChanged -= Sort;
        }


        public void Sort(ICard[] cards)
        {
            if (cards == null)
                throw new ArgumentException("Can't sort a card list null");

            var _lastPos = cards.Length - 1;
            var _lastCard = cards[_lastPos];
            var _gravPos = _graveyardPosition.position + new Vector3(0, 0, -5);
            var _backGravPos = _graveyardPosition.position;

            _lastCard.MoveToWithZ(_gravPos, 4); // Todo Movement Speed

            for (var i = 0; i < cards.Length - 1; i++)
            {
                var card = cards[i];
                card.MoveToWithZ(_backGravPos, 4); // Todo Movement Speed
            }
        }
    }
}