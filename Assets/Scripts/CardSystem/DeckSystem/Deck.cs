using System;
using System.Collections.Generic;
using System.Linq;
using Card.CardPile;
using Card.CardPile.Graveyard;
using Extensions;
using ServiceLocator;
using ToyoSystem;
using UnityEngine;
using Zenject;

namespace Card.DeckSystem
{
    public class Deck : MonoBehaviour, IDeck, ICardPile
    {
        public Lazy<List<ICard>> _cards = new(new List<ICard>());
        public List<ICard> Cards => _cards.Value;
        
        private IFullToyo _fullToyo;
        public IFullToyo FullToyo => _fullToyo;
        
        private ICardPile _graveyard;
        public ICardPile Graveyard => _graveyard;
        
        public List<int> _allCardsID;
        public List<int> AllCardIDS => _allCardsID ??= new List<int>();

        [Inject]
        public void Construct(IFullToyo fullToyo, CardGraveyard graveyard)
        {
            _fullToyo = fullToyo;
            _graveyard = graveyard;
        }
        
        Action<ICard[]> ICardPile.OnPileChanged
        {
            get => OnPileChanged;
            set => OnPileChanged = value;
        }
        
        public void AddCard(ICard _card)
        {
            Cards.Add(_card);
            _card.transform.SetParent(transform);
            _card.transform.position = transform.position;
            _card.Disable();
            if(!_allCardsID.Contains(_card.CardID))
                _allCardsID.Add(_card.CardID);
            NotifyPileChange();
        }

        public void RemoveCard(ICard _card)
        {
            Cards.Remove(_card);
            NotifyPileChange();
        }

        private const int NumberRequiredForSynergy = 5;
        private const int MaxNumberDebugCards = 10;
        
        public bool HasSynergyCards()
        {
            Dictionary<int, int> _countEachPart = FullToyo.CountEachPartToyo();

            return _countEachPart.Any(_part => _part.Value >= NumberRequiredForSynergy);
        }
        
        public void InitializeFullToyo(FullToyoSO fullToyoSo)
        {
            FullToyo.InitializeToyoDebug(this, fullToyoSo);
            InitializeDeckFromToyo();
            return; //Todo Get Toyo from Database
            
        }

        public void InitializeFullToyo()
        {
            FullToyo.InitializeToyo(this);
            InitializeDeckFromToyo();
            return; //Todo Get Toyo from Database
        }
        

        public void InitializeDeckFromToyo()
        {
            if (FullToyo != null)
            {
                var _toyoParts = FullToyo.ToyoParts;
                foreach (var _part in _toyoParts)
                    AddCardsFromPart(_part.Value.CardsFromPiece);
            }

            if (Cards.Count <= 0)
                GenerateDeckDebug();
            
            
        }

        void AddCardsFromPart(List<ICard> _cards)
        {
            foreach (var _card in _cards)
                AddCard(_card);
        }

        public ICard GetTopCard()
        {
            if (Cards?.Count <= 0)
            {
                ShuffleDeckFromGraveyard();
            }
            var _card = Cards?.First();
            RemoveCard(_card);
            
            return _card;
        }

        void ShuffleDeckFromGraveyard()
        {
            var _cards = Graveyard.Cards;
            for (int i = 0; i < _cards.Count; i++)
            {
                AddCard(_cards[i]);
                Graveyard.RemoveCard(_cards[i]);
                
            }
            ShuffleDeck();

        }

        public void ShuffleDeck()
        {
            Cards.Shuffle();
        }
    
        void GenerateDeckDebug()
        {
            var _deckDebug = Resources.Load<DeckDebugData>("DeckDebugData");
            var _deck = _deckDebug.Deck;
            var _cardPackIndex = 0;
            while (Cards.Count <= MaxNumberDebugCards)
            {
                for (var _index = 0; _index < _deck.Count; _index++)
                {
                    var _currentCardData = _deck[_index];
                    var cardGo = Instantiate(CardUtils.GetCardPrefabByType(_currentCardData.CardType), transform);
                    cardGo.name = "Card_" + _cardPackIndex+ "_" +_index;
                    var card = cardGo.GetComponent<ICard>();
                    card.CardData = _currentCardData;
                    AddCard(card);
                    cardGo.transform.position = Locator.GetGlobalConfig().deckPosition.position;
                }
                _cardPackIndex++;
            }
        }

        private event Action<ICard[]> OnPileChanged = _ => { };

        protected virtual void Clear()
        {
            var childCards = GetComponentsInChildren<ICard>();
            foreach (var uiCardHand in childCards)
                Destroy(uiCardHand.gameObject);

            Cards.Clear();
        }

        public void NotifyPileChange()
        {
            OnPileChanged?.Invoke(Cards.ToArray());
        }


    }
}