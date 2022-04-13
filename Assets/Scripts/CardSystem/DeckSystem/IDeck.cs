using System.Collections.Generic;
using ToyoSystem;

namespace Card.DeckSystem
{
    public interface IDeck
    {
        List<ICard> CardList { get; set; }
        IFullToyo FullToyo { get; }
        bool HasSynergyCards();
        void InitializeDeckFromToyo();
        ICard GetTopCard();

    }
}