using System;
using System.Collections.Generic;

namespace Card.CardPile
{
    public interface ICardPile
    {
        public List<ICard> Cards { get; }
        Action<ICard[]> OnPileChanged { get; set; }
        void AddCard(ICard card);
        void RemoveCard(ICard card);
    }
}