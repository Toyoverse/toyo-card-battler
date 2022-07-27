using System;
using System.Collections.Generic;
using System.Linq;
using Card.CardPile;
using CombatSystem;
using Fusion;
using Player;
using ServiceLocator;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Card
{
    public static class CardUtils
    {
        private static PlayerNetworkManager _playerNetworkManager => Locator.GetGlobalConfig().PlayerNetworkManager;

        public static void ValidateCard(this ICard card)
        {
            if (card == null) throw new ArgumentNullException("Card is null");
        }

        public static bool ValidateBoundEffects(this ICard card, List<EffectData> _effectList)
            => CanIPlayThisCard(card, _effectList);
        
        public static void ValidateCardAP(this ICard card, PlayerRef _playerRef)
        {
            var _player = _playerNetworkManager.GetPlayer(_playerRef);
            var _ap = _player.MyPlayerApModel.Ap;
            if (card.CardData.ApCost > _ap) NotEnoughAP();
        }

        public static void NotEnoughAP()
        {
            //Todo - UX for not enough AP
            if (Locator.GetGlobalConfig().IgnoreAPCost) return;
            throw new ArgumentNullException("Not Enough AP");
        }

        public static List<ICard> CreateCardsFromCardData(List<CardData> cards, ICardPile handler)
        {
            return cards.Select(_card => InstantiateCard(handler, _card)).ToList();
        }

        static ICard InstantiateCard(ICardPile handler, CardData cardData)
        {
            var cardGo = Object.Instantiate(GetCardPrefabByType(cardData.CardType), Locator.GetGlobalConfig().deckPosition);
            cardGo.name = "Card_" +cardData.toyoPart +"_"+cardData.CardName;
            var card = cardGo.GetComponent<ICard>();
            card.CardData = cardData;
            handler.AddCard(card);
            cardGo.transform.position = Locator.GetGlobalConfig().deckPosition.position;
            return card;
        }
        
        /*
         * We only need to send the ID throught the network, instead of sending all the data.
         * Todo : Only for test, optimize later
         */
        public static ICard FindCardByID(int _id)
        {
            var _cards = GameObject.FindObjectsOfType<CardComponent>(true);

            //var _cards = GlobalConfig.Instance.DeckDatabaseSo.AllCards;

            return _cards.FirstOrDefault(_card => _card.CardID == _id);
        }
        
                
        public static List<ICard> FindCardsByIDs(List<int> _id)
        {
            var _cards = GameObject.FindObjectsOfType<CardComponent>();
            return _cards.Where(_card => _id.Contains(_card.CardID)).Cast<ICard>().ToList();
        }
        
        public static GameObject GetCardPrefabByType(CARD_TYPE _type)
        {
            return _type switch
            {
                CARD_TYPE.HEAVY => Locator.GetGlobalConfig().GlobalCardSettings.heavyCardPrefab,
                CARD_TYPE.FAST => Locator.GetGlobalConfig().GlobalCardSettings.fastCardPrefab,
                CARD_TYPE.DEFENSE => Locator.GetGlobalConfig().GlobalCardSettings.defenseCardPrefab,
                CARD_TYPE.BOND => Locator.GetGlobalConfig().GlobalCardSettings.bondCardPrefab,
                CARD_TYPE.SUPER => Locator.GetGlobalConfig().GlobalCardSettings.superCardPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(_type), _type, null)
            };
        }
        
        private static bool CanIPlayThisCard(ICard _card, List<EffectData> _effectList)
        {
            for (var i = 0; i < _effectList.Count; i++)
            {
                var effect = _effectList[i];
                if (effect.EffectType != EFFECT_TYPE.RULE_MOD) continue;
                if (effect.excludedTypes.All(type => type != _card.CardData.CardType)) continue;
                BoundSystem.CheckRemoveEffect(ref _effectList, effect);
                return false;
            }
            return true;
        }
    }
}