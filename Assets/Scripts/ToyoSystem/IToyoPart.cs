using System;
using System.Collections.Generic;
using Card;

namespace ToyoSystem
{
    public interface IToyoPart
    {
        TOYO_PIECE ToyoPiece { get; }
        TOYO_TECHNOALLOY ToyoTechnoalloy { get; }
        List<ToyoPartStat> PartStat { get; }
        List<ToyoPartStat> BonusPartStat { get; }
        List<ICard> CardsFromPiece { get; set; }
        float Experience { get; set; }
        int Level { get; set; }
    }
}