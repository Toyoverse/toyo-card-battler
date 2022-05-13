using System;
using System.Collections.Generic;
using System.Linq;
using APSystem;
using Card.CardPile;
using Fusion;
using Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Card
{
    public static class CardUtils
    {
        public static void ValidateCard(this ICard card)
        {
            if (card == null) /*throw new ArgumentNullException("Card is null");*/ throw new Exception();
        }
        
        public static void ValidateCardAP(this ICard card, PlayerRef _playerRef)
        {
            var _player = PlayerNetworkManager.GetPlayer(_playerRef);
            var _ap = _player.MyPlayerAP.GetAP();
            if (card.CardData.ApCost > _ap) NotEnoughAP();
        }

        public static void NotEnoughAP()
        {
            //Todo - UX for not enough AP
            if (GlobalConfig.Instance.IgnoreAPCost) return;
            throw new ArgumentNullException("Not Enough AP");
        }

        public static List<ICard> CreateCardsFromCardData(List<CardData> cards, ICardPile handler)
        {
            return cards.Select(_card => InstantiateCard(handler, _card)).ToList();
        }

        static ICard InstantiateCard(ICardPile handler, CardData cardData)
        {
            var cardGo = Object.Instantiate(GlobalConfig.Instance.cardDefaultPrefab, GlobalConfig.Instance.deckPosition);
            cardGo.name = "Card_" +cardData.toyoPart +"_"+cardData.LocalizedName;
            var card = cardGo.GetComponent<ICard>();
            card.CardData = cardData;
            card.CardID = cardData.Id;
            handler.AddCard(card);
            cardGo.transform.position = GlobalConfig.Instance.deckPosition.position;
            return card;

        }
        
        /*
         * We only need to send the ID throught the network, instead of sending all the data.
         * Todo : Only for test, optimize later
         */
        public static ICard FindCardByID(int _id)
        {
            var _cards = GameObject.FindObjectsOfType<CardComponent>();
            return _cards.FirstOrDefault(_card => _card.ID == _id);
        }
        
        public static List<ICard> FindCardsByIDs(List<int> _id)
        {
            var _cards = GameObject.FindObjectsOfType<CardComponent>();
            return _cards.Where(_card => _id.Contains(_card.ID)).Cast<ICard>().ToList();
        }
    }
}