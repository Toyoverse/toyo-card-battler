using System;
using System.Collections.Generic;
using Card;

namespace ToyoSystem
{
    public interface IToyoPart
    {
        TOYO_PIECE ToyoPiece { get; }
        TOYO_TECHNOALLOY ToyoTechnoalloy { get; }
        Tuple<TOYO_STAT, float> PartStat { get; }
        List<ICard> CardsFromPiece { get; set; }
    }
}