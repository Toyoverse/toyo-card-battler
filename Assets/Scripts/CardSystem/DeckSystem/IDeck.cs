using System.Collections.Generic;
using ToyoSystem;

namespace Card.DeckSystem
{
    public interface IDeck
    {
        IFullToyo FullToyo { get;}
        bool HasSynergyCards();
        void InitializeDeckFromToyo();
        ICard GetTopCard();
        void ShuffleDeck();
        void InitializeFullToyo();
    }
}