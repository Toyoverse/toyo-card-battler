using System.Collections;
using Card;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerHand
{
    public class PlayerHandUtils : MonoBehaviour
    {
        #region Settings

        private int Count { get; set; }

        [SerializeField] [Tooltip("Prefab of the Card C#")]
        private GameObject cardPrefabCs;

        [SerializeField] [Tooltip("World point where the deck is positioned")]
        private Transform deckPosition;

        [SerializeField] [Tooltip("Game view transform")]
        private Transform gameView;

        private IPlayerHand PlayerHand { get; set; }

        #endregion

        #region Unitycallbacks

        private void Awake()
        {
            PlayerHand = GlobalConfig.Instance.PlayerReferences.hand.GetComponent<IPlayerHand>();
        }

        private IEnumerator Start()
        {
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
            var cardGo = Instantiate(cardPrefabCs, gameView);
            cardGo.name = "Card_" + Count;
            var card = cardGo.GetComponent<ICard>();
            card.transform.position = deckPosition.position;
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