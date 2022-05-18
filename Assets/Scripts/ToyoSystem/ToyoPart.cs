using System;
using System.Collections.Generic;
using Card;
using Card.CardPile;
using Card.DeckSystem;
using UnityEngine;

namespace ToyoSystem
{
    public class ToyoPart : IToyoPart
    {
        public TOYO_PIECE ToyoPiece { get; set; }
        public TOYO_TECHNOALLOY ToyoTechnoalloy { get; set; }
        public List<ToyoPartStat> PartStat { get; set; }
        public List<ToyoPartStat> BonusPartStat { get; set; }
        public List<ICard> CardsFromPiece { get; set; }

        private ToyoPartSO PartData;

        public ToyoPart(ToyoPartSO partData, ICardPile handler)
        {
            PartData = partData;
            ToyoPiece = partData.ToyoPiece;
            ToyoTechnoalloy = partData.ToyoTechnoalloy;
            PartStat = partData.ToyoPartStat;
            SetPieceInCardData();
            CardsFromPiece = CardUtils.CreateCardsFromCardData(PartData.CardsFromPiece, handler);
        }
        
        public ToyoPart(ToyoPartSO partData)
        {
            PartData = partData;
            ToyoPiece = partData.ToyoPiece;
            ToyoTechnoalloy = partData.ToyoTechnoalloy;
            PartStat = partData.ToyoPartStat;
        }

        void SetPieceInCardData()
        {
            foreach (var card in PartData.CardsFromPiece)
                card.toyoPart = ToyoPiece;
        }
        

    }
}