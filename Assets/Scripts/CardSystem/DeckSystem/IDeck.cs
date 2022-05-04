using System.Collections.Generic;
using ToyoSystem;

namespace Card.DeckSystem
{
    public interface IDeck
    {
        IFullToyo FullToyo { get; set; }
        bool HasSynergyCards();
        void InitializeDeckFromToyo();
        ICard GetTopCard();
        void ShuffleDeck();

    }
}