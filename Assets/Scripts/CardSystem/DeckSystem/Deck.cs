using System;
using System.Collections.Generic;
using System.Linq;
using ToyoSystem;
using UnityEngine;

namespace Card.DeckSystem
{
    public class Deck : MonoBehaviour, IDeck
    {
        public List<ICard> CardList { get; set; }
        public IFullToyo FullToyo { get; }

        private const int NumberRequiredForSynergy = 5;
        
        public bool HasSynergyCards()
        {
            Dictionary<string, int> _countEachPart = CountEachPart();

            return _countEachPart.Any(_part => _part.Value >= NumberRequiredForSynergy);
        }

        private void Awake()
        {
            InitializeFullToyo();
            InitializeDeckFromToyo();
        }

        void InitializeFullToyo()
        {
            return; //Todo Get Toyo from Database
        }

        public void InitializeDeckFromToyo()
        {
            if (FullToyo != null)
            {
                var _toyoParts = FullToyo.ToyoParts;
                CardList = new List<ICard>();

                foreach (var _part in _toyoParts)
                    CardList.AddRange(_part.Value.CardsFromPiece);
            }
            else
                CardList = new List<ICard>();

            if (CardList.Count <= 0)
                GenerateDeckDebug();
        }

        public ICard GetTopCard()
        {
            var _card = CardList?.First();
            CardList?.RemoveAt(0);
            return _card;
        }

        void GenerateDeckDebug()
        {
            var _deckDebug = Resources.Load<DeckDebugData>("DeckDebugData");
            var _deck = _deckDebug.Deck;
            var _cardPackIndex = 0;
            while (CardList.Count <= 30)
            {
                for (var _index = 0; _index < _deck.Count; _index++)
                {
                    var cardGo = Instantiate(GlobalConfig.Instance.cardDefaultPrefab, GlobalConfig.Instance.gameView);
                    cardGo.name = "Card_" + _cardPackIndex+ "_" +_index;
                    var card = cardGo.GetComponent<ICard>();
                    card.CardData = _deck[_index];
                    CardList.Add(card);
                    cardGo.transform.position = GlobalConfig.Instance.deckPosition.position;
                    cardGo.gameObject.SetActive(false);
                }
                _cardPackIndex++;
            }
        }

        Dictionary<string, int> CountEachPart()
        {
            Dictionary<string, int> CountEachPart = new Dictionary<string, int>();

            var _listToyoData = FullToyo.ToyoIDs.ListToyoData;
            
            foreach (var _toyoIDData in _listToyoData)
            {
                if (CountEachPart.ContainsKey(_toyoIDData.Id))
                {
                    var _value = CountEachPart[_toyoIDData.Id];
                    _value++;
                    CountEachPart[_toyoIDData.Id] = _value;
                }
                else
                {
                    CountEachPart.Add(_toyoIDData.Id, 1);
                }
            }

            return CountEachPart;
        }
    }
}