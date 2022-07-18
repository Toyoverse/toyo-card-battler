using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Extensions;
using Fusion;
using Player;
using PlayerHand;
using TMPro;
using Tools.Extensions;
using UnityEngine;

namespace Card.QueueSystem
{
    public class CardQueueSystem : MonoBehaviour
    {
        public TextMeshProUGUI playerQueueSize;
        public TextMeshProUGUI enemyQueueSize;
        public TextMeshProUGUI currentCardDuration;
        public TextMeshProUGUI playerCurrentCombo;
        public TextMeshProUGUI enemyCurrentCombo; 
        
        public GameObject playerComboObj;
        public GameObject enemyComboObj;

        private ComboSystem _comboSystem;
        public ComboSystem ComboSystem => _comboSystem ??= this.LazyNew(ref _comboSystem);
        
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
            {
                ComboSystem.ComboPlus(false);
                PlayerNetworkManager.Instance.IsHostCardPlaying = true;
                PopFromPlayerQueue();
            }
            else if (GetEnemyQueueSize() > 0) // Else added to avoid nullpointer because of null CardBeingExecuted - It will be fixed in the interrupt system
            {
                ComboSystem.ComboPlus(true);
                PlayerNetworkManager.Instance.IsHostCardPlaying = false;
                PopFromEnemyQueue();
            }
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
                    else
                        ComboSystem.ComboBreak();
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
                playerCurrentCombo.text = ComboSystem.GetNetworkedPlayerCurrentCombo().ToString();
                enemyCurrentCombo.text = ComboSystem.GetNetworkedEnemyCurrentCombo().ToString();
            }
            else
            {
                enemyQueueSize.text = GetPlayerQueueSize().ToString();
                playerQueueSize.text = GetEnemyQueueSize().ToString();
                enemyCurrentCombo.text = ComboSystem.GetNetworkedPlayerCurrentCombo().ToString();
                playerCurrentCombo.text = ComboSystem.GetNetworkedEnemyCurrentCombo().ToString();
            }
            
            currentCardDuration.text = Mathf.Round(GetNetworkedCurrentCardDuration()).ToString();
            ActiveComboUI();
        }

        private void ActiveComboUI()
        {
            if (FusionLauncher.IsServer)
            {
                if (PlayerNetworkManager.Instance.PlayerCurrentCombo > 0)
                {
                    if (!playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(true);
                }
                else
                {
                    if(playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(false);
                }

                if (PlayerNetworkManager.Instance.EnemyCurrentCombo > 0)
                {
                    if(!enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(true);
                }
                else
                {
                    if(enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(false);
                }
            }
            else
            {
                if (PlayerNetworkManager.Instance.PlayerCurrentCombo > 0)
                {
                    if (!enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(true);
                }
                else
                {
                    if(enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(false);
                }

                if (PlayerNetworkManager.Instance.EnemyCurrentCombo > 0)
                {
                    if(!playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(true);
                }
                else
                {
                    if(playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(false);
                }
            }
        }
    }
}