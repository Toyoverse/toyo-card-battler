using System.Collections.Generic;
using System.Linq;
using Player;
using CardSystem.PlayerHand;
using UnityEngine;
using Zenject;

namespace Card.QueueSystem
{
    [RequireComponent(typeof(CardQueueSystemPresenter))]
    public class CardQueueSystemModel : MonoBehaviour
    {
        private CardQueueSystemPresenter _myCardQueueSystemPresenter;
        private ComboSystem _comboSystem;
        private IPlayerHand _playerHand;

        private SignalBus _signalBus;
        private PlayerNetworkManager _playerNetworkManager;
        
        private ICard _cardBeingExecuted;
        private float _currentCardDuration = 0.0f;
        private bool _isCurrentCardEnemy;

        #region CallBacks
        
        private void OnEnable()
        {
            _playerHand.OnAddCardToQueue += AddPlayedCardToQueue;
        }

        private void OnDisable()
        {
            _playerHand.OnAddCardToQueue -= AddPlayedCardToQueue;
        }

        private void Awake()
        {
            _myCardQueueSystemPresenter = GetComponent<CardQueueSystemPresenter>();
        }
        
        private void Update()
        {
            if (_playerNetworkManager == null) return;
            if (FusionLauncher.IsServer)
            {
                if(_currentCardDuration > 0.0f)
                    _currentCardDuration -= Time.deltaTime;
                else
                    _cardBeingExecuted = null;

                if (_cardBeingExecuted == null)
                {
                    if(GetCardQueuePlayer().Count > 0 || GetCardQueueEnemy().Count > 0)
                        PlayNextCardInQueue();
                    else
                        ComboSystem.ComboBreak();
                }
            }
            
            CallUpdateUI();
        }

        #endregion

        [Inject]
        public void Construct(ComboSystem comboSystem, IPlayerHand playerHand, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<PlayerNetworkInitializedSignal>(x => _playerNetworkManager = x.PlayerNetworkManager);
            
            _playerHand = playerHand;
            _comboSystem = comboSystem;
        }
        
        private void AddToPlayerQueue(ICard _card) => GetCardQueuePlayer().Add(_card);
        private void AddToEnemyQueue(ICard _card) => GetCardQueueEnemy().Add(_card);
        private void RemoveFromPlayerQueue(ICard _card) => GetCardQueuePlayer().Remove(_card);
        private void RemoveFromEnemyQueue(ICard _card) => GetCardQueueEnemy().Remove(_card);

        private void PopFromPlayerQueue()
        {
            _cardBeingExecuted = GetCardQueuePlayer().First();
            if (_cardBeingExecuted == null) return;
            RemoveFromPlayerQueue(_cardBeingExecuted);
            _playerHand.OnCardPlayed?.Invoke(_cardBeingExecuted);
            SetCurrentCardDuration();
        }

        private void PopFromEnemyQueue()
        {
            _cardBeingExecuted = GetCardQueueEnemy().First();
            if (_cardBeingExecuted == null) return;
            RemoveFromEnemyQueue(_cardBeingExecuted);
            _playerHand.OnNetworkCardPlayed?.Invoke(_cardBeingExecuted);
            SetCurrentCardDuration();
        }

        private void AddPlayedCardToQueue(ICard _card)
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

        /*
         * Todo : Interrupt System
         */
        private void PlayNextCardInQueue()
        {
            if (GetPlayerQueueSize() > 0)
            {
                ComboSystem.ComboPlus(false);
                _playerNetworkManager.IsHostCardPlaying = true;
                PopFromPlayerQueue();
            }
            else if (GetEnemyQueueSize() > 0) // Else added to avoid nullpointer because of null CardBeingExecuted - It will be fixed in the interrupt system
            {
                ComboSystem.ComboPlus(true);
                _playerNetworkManager.IsHostCardPlaying = false;
                PopFromEnemyQueue();
            }
        }

        private void CallUpdateUI()
        {
            var playerQueueSize = GetPlayerQueueSize().ToString();
            var enemyQueueSize = GetEnemyQueueSize().ToString();
            var playerCurrentCombo = GePlayerCurrentCombo().ToString();
            var enemyCurrentCombo = GetEnemyCurrentCombo().ToString();
            var currentCardDuration = Mathf.Round(GetNetworkedCurrentCardDuration()).ToString();
            
            _myCardQueueSystemPresenter.OnUpdateUI?.Invoke(
                playerQueueSize, enemyQueueSize, playerCurrentCombo, enemyCurrentCombo, currentCardDuration);
            
            CallActiveComboUI();
        }

        private void CallActiveComboUI()
        {
            _myCardQueueSystemPresenter.OnActivateComboUI?.Invoke(_playerNetworkManager);
        }

        #region Getters/Setters
        
        public float CurrentCardDuration => _currentCardDuration;
        public ComboSystem ComboSystem => _comboSystem;
        
        private List<ICard> GetCardQueuePlayer() => _playerNetworkManager.PlayerCardQueue;
        private List<ICard> GetCardQueueEnemy() => _playerNetworkManager.EnemyCardQueue;
        private int GetPlayerQueueSize() => _playerNetworkManager.PlayerQueueSize;
        private int GetEnemyQueueSize() => _playerNetworkManager.EnemyQueueSize;
        private int GePlayerCurrentCombo() => _playerNetworkManager.PlayerCurrentCombo;
        private int GetEnemyCurrentCombo() => _playerNetworkManager.EnemyCurrentCombo;
        private float GetNetworkedCurrentCardDuration() => _playerNetworkManager.CurrentCardDuration;
        private void SetCurrentCardDuration() => _currentCardDuration = _cardBeingExecuted.CardData.CardDuration; 

        #endregion
    }
}