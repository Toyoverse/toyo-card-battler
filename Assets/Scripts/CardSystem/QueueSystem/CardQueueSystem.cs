using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Player;
using PlayerHand;
using TMPro;
using UnityEngine;

namespace Card.QueueSystem
{
    public class CardQueueSystem : MonoBehaviour
    {
        public TextMeshProUGUI playerQueueSize;
        public TextMeshProUGUI enemyQueueSize;
        public TextMeshProUGUI currentCardDuration;
        
        private IPlayerHand _playerHand;
        public IPlayerHand PlayerHand => _playerHand ??= this.LazyFindOfType(ref _playerHand);

        private void OnEnable()
        {
            PlayerHand.OnAddCardToQueue += AddPlayedCardToQueue;
        }

        private void OnDisable()
        {
            PlayerHand.OnAddCardToQueue -= AddPlayedCardToQueue;
        }
        
        public List<ICard> GetCardQueuePlayer() => PlayerNetworkManager.Instance.PlayerCardQueue;
        public List<ICard> GetCardQueueEnemy() => PlayerNetworkManager.Instance.EnemyCardQueue;

        private int GetPlayerQueueSize() => PlayerNetworkManager.Instance.PlayerQueueSize;
        private int GetEnemyQueueSize() => PlayerNetworkManager.Instance.EnemyQueueSize;

        public void AddToPlayerQueue(ICard _card) => GetCardQueuePlayer().Add(_card);
        public void AddToEnemyQueue(ICard _card) => GetCardQueueEnemy().Add(_card);
        
        public void RemoveFromPlayerQueue(ICard _card) => GetCardQueuePlayer().Remove(_card);
        public void RemoveFromEnemyQueue(ICard _card) => GetCardQueueEnemy().Remove(_card);

        public float GetOfflineCurrentCardDuration() => CurrentCardDuration;
        private float GetNetworkedCurrentCardDuration() => PlayerNetworkManager.Instance.CurrentCardDuration;

        public void PopFromPlayerQueue()
        {
            CardBeingExecuted = GetCardQueuePlayer().First();
            if (CardBeingExecuted == null) return;
            RemoveFromPlayerQueue(CardBeingExecuted);
            PlayerHand.OnCardPlayed?.Invoke(CardBeingExecuted);
            SetCurrentCardDuration();
        }
        
        public void PopFromEnemyQueue()
        {
            CardBeingExecuted = GetCardQueueEnemy().First();
            if (CardBeingExecuted == null) return;
            RemoveFromEnemyQueue(CardBeingExecuted);
            PlayerHand.OnNetworkCardPlayed?.Invoke(CardBeingExecuted);
            SetCurrentCardDuration();
        }

        public ICard CardBeingExecuted;
        private float CurrentCardDuration = 0.0f;
        private bool IsCurrentCardEnemy;

        public void AddPlayedCardToQueue(ICard _card)
        {
            if (_card == null)
            {
                Debug.Log("Card is Null in Queue");
                return;
            }
            AddToPlayerQueue(_card);
        }
        
        public void AddEnemyCardToQueue(ICard _card)
        {
            if (_card == null)
            {
                Debug.Log("Card is Null in Queue");
                return;
            }
            AddToEnemyQueue(_card);
        }

        private void SetCurrentCardDuration() => CurrentCardDuration = CardBeingExecuted.CardData.CardDuration; 
    
        /*
         * Todo : Interrupt System
         */
        private void PlayNextCardInQueue()
        {
            if (GetPlayerQueueSize() > 0)
                PopFromPlayerQueue();
            else if (GetEnemyQueueSize() > 0) // Else added to avoid nullpointer because of null CardBeingExecuted - It will be fixed in the interrupt system
                PopFromEnemyQueue();
        }

        private void Update()
        {
            if (FusionLauncher.IsServer)
            {
                if(CurrentCardDuration > 0.0f)
                    CurrentCardDuration -= Time.deltaTime;
                else
                    CardBeingExecuted = null;
            
                if (CardBeingExecuted == null)
                {
                    if(GetCardQueuePlayer().Count > 0 || GetCardQueueEnemy().Count > 0)
                        PlayNextCardInQueue();
                }

            }
            UpdateUI();

        }

        private void UpdateUI()
        {
            if (FusionLauncher.IsServer)
            {
                playerQueueSize.text = GetPlayerQueueSize().ToString();
                enemyQueueSize.text = GetEnemyQueueSize().ToString();
            }
            else
            {
                enemyQueueSize.text = GetPlayerQueueSize().ToString();
                playerQueueSize.text = GetEnemyQueueSize().ToString();
            }
            
            currentCardDuration.text = Mathf.Round(GetNetworkedCurrentCardDuration()).ToString();

        }
    }
}