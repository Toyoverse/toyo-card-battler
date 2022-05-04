using System.Collections;
using Card;
using Card.DeckSystem;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerHand
{
    public class PlayerHandUtils : MonoBehaviour
    {
        #region Settings

        private int Count { get; set; }

        private IPlayerHand PlayerHand { get; set; }
        private IDeck Deck { get; set; }

        #endregion

        #region Unitycallbacks

        private void Awake()
        {
            /*
            PlayerHand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>();
            Deck = GlobalConfig.Instance.battleReferences.deck.GetComponent<IDeck>();
            */
        }
        
        private IEnumerator Start()
        {
            PlayerHand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>() ?? FindObjectOfType<PlayerHand>();;
            Deck = GlobalConfig.Instance.battleReferences.deck.GetComponent<IDeck>() ?? FindObjectOfType<Deck>();
            Deck.ShuffleDeck();
            yield return new WaitForSeconds(1f);
            //starting cards
            for (var i = 0; i < 5; i++)
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

        //Old
        void DrawCardDebug()
        {
            var cardGo = Instantiate(GlobalConfig.Instance.cardDefaultPrefab, GlobalConfig.Instance.gameView);
            cardGo.name = "Card_" + Count;
            var card = cardGo.GetComponent<ICard>();
            card.transform.position = GlobalConfig.Instance.deckPosition.position;
            Count++;
            PlayerHand.AddCard(card);
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