using System;
using System.Collections;
using Card;
using Card.DeckSystem;
using Extensions;
using Fusion;
using ServiceLocator;
using Tools.Extensions;
using ToyoSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CardSystem.PlayerHand
{
    public class PlayerHandUtils : MonoBehaviour
    {

        public static bool IsHandDrawed;
        #region Settings

        private int Count { get; set; }

        private IPlayerHand _playerHand;
        public IPlayerHand PlayerHand => _playerHand;
        
        private IDeck _deck;
        public IDeck Deck => _deck;

        [Inject]
        public void Construct(IDeck deck, IPlayerHand playerHand)
        {
            _playerHand = playerHand;
            _deck = deck;
        }

        #endregion

        #region Unitycallbacks

        private void OnEnable()
        {
            _playerHand.OnAddCardToQueue += AutomaticDrawCard;
        }
        
        private void OnDisable()
        {
            _playerHand.OnAddCardToQueue -= AutomaticDrawCard;
            IsHandDrawed = false;
        }

        private void AutomaticDrawCard(ICard card)
        {
            if (!Locator.GetGlobalConfig().GlobalCardSettings.automaticDrawCards) return;
            
            while(_playerHand.Cards.Count < Locator.GetGlobalConfig().GlobalCardSettings.handSize)
                DrawCard();
        }

        public IEnumerator DrawFirstHand(FullToyoSO fullToyoSo)
        {
            while (!FusionLauncher.IsConnected && FusionLauncher.GameMode != GameMode.Single)
                yield return null;
            IsHandDrawed = true;
            Deck?.InitializeFullToyo(fullToyoSo);
            Deck?.ShuffleDeck();
            yield return new WaitForSeconds(1f);
            //starting cards
            for (var i = 0; i < Locator.GetGlobalConfig().GlobalCardSettings.handSize; i++)
            {
                yield return new WaitForSeconds(0.2f);
                DrawCard();
            }
            
        }

        #endregion

        #region Functions

        public void DrawCard()
        {
            var _card = Deck?.GetTopCard();
            _card?.gameObject.SetActive(true);
            Count++;
            PlayerHand.AddCard(_card);
        }

        public void PlayCard()
        {
            if (PlayerHand.Cards.Count > 0)
            {
                var randomCard = PlayerHand.Cards.RandomItem();
                PlayerHand.PlayCard(randomCard);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) DrawCard();
            if (Input.GetKeyDown(KeyCode.Space)) PlayCard();
            if (Input.GetKeyDown(KeyCode.Escape)) Restart();
        }


        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        //--------------------------------------------------------------------------------------------------------------
    }
}