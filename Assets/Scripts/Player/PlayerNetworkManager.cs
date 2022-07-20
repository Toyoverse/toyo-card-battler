using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Card.QueueSystem;
using CombatSystem;
using ExitGames.Client.Photon;
using Tools.Extensions;
using Fusion;
using HealthSystem;
using Multiplayer;
using UnityEngine;

namespace Player
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        [Networked]
        public bool IsWorldReady { get; set; }
        
        public GameObject PlayerUI => GlobalConfig.Instance.PlayerUI;
        private HealthModel _playerHealthModel;
        
        public GameObject EnemyUI =>  GlobalConfig.Instance.EnemyUI;
        private HealthModel _enemyHealthModel;

        [Networked, Capacity(4)] public NetworkArray<GameState> LastGameStates => default;
        [Networked] public int CurrentStateID { get; set; }

        private static List<PlayerNetworkObject> _players;
        public List<PlayerNetworkObject> Players => _players ??= FindObjectsOfType<PlayerNetworkObject>()?.ToList();
        
        private void Awake() => Instance = this;

        #region Statics
        
        public static PlayerNetworkManager Instance;

        public static void AddPlayer(PlayerNetworkObject playerNetworkObject) => Instance.Players.Add(playerNetworkObject);
        
        public static void RemovePlayer(PlayerNetworkObject playerNetworkObject) => Instance.Players.Remove(playerNetworkObject);

        public static PlayerNetworkObject GetPlayer(PlayerRef playerRef) => Instance.Players.FirstOrDefault(_player => _player.NetworkPlayerRef.PlayerId == playerRef.PlayerId);
        
        public static PlayerNetworkObject GetLocalPlayer() => Instance.Players.FirstOrDefault(_player => !_player.IsEnemy);
        
        public static PlayerNetworkObject GetEnemy() => Instance.Players.FirstOrDefault(_player => _player.IsEnemy);

        public static List<ICard> GetCardQueuePlayer() => Instance.PlayerCardQueue;
        
        public static List<ICard> GetCardQueueEnemy() => Instance.EnemyCardQueue;
        
        #endregion

        #region Host

        public GameState GetCurrentGameState() => GetGameState(CurrentStateID);
        
        private GameState GetGameState(int _currentStateID) => LastGameStates.Get(_currentStateID);

        private CardQueueSystem _cardQueueSystem;
        public CardQueueSystem CardQueueSystem => _cardQueueSystem ??= FindObjectOfType<CardQueueSystem>();

        private List<int> AllCardIDS = new();
        public List<ICard> AllCards = new();

        public List<ICard> PlayerCardQueue = new();
        public List<ICard> EnemyCardQueue = new();

        [Networked] public int PlayerQueueSize { get; set; }
        [Networked] public int EnemyQueueSize { get; set; }
        [Networked] public float CurrentCardDuration { get; set; }
        [Networked] public int PlayerCurrentCombo { get; set; }
        [Networked] public int EnemyCurrentCombo { get; set; }
        [Networked] public bool IsHostCardPlaying { get; set; }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority)
            {
                if (CardQueueSystem != null)
                {
                    CurrentCardDuration = CardQueueSystem.GetOfflineCurrentCardDuration();
                    PlayerCurrentCombo = CardQueueSystem.ComboSystem.GetOfflinePlayerCurrentCombo();
                    EnemyCurrentCombo = CardQueueSystem.ComboSystem.GetOfflineEnemyCurrentCombo();
                }

                PlayerQueueSize = PlayerCardQueue.Count;
                EnemyQueueSize = EnemyCardQueue.Count;
                if(!IsWorldReady)
                    IsWorldReady = true;
            }
        }


        /*
         * Todo: VERY BAD -- Temporary - Changing for unity adressables next sprint
         */
        public void SetFirstGameState(List<int> cardIDS)
        {
            AllCardIDS.AddRange(cardIDS);
            AllCards.AddRange(CardUtils.FindCardsByIDs(cardIDS));
            
            var state = new GameState();
            for (int i = 0; i < AllCardIDS.Count; i++)
                state.AllCardsThisMatch.Set(i,AllCardIDS[i]);
            
            NextGameState();
            LastGameStates.Set(CurrentStateID, state);
        }
        
        public void SetGameState(PlayerInputData _playerInput)
        {
            NextGameState();
            var state = new GameState
            {
                GameStatePlayerRef = _playerInput.PlayerRef,
                newCardId = _playerInput.PlayedCardID
            };
            LastGameStates.Set(CurrentStateID, state);
            
            if (Object.HasStateAuthority)
                CheckForCardsPlayed();
        }

        /*
         * Main location for card synchronization
         */
        private void CheckForCardsPlayed()
        {
            if (GetCurrentGameState().GameStatePlayerRef == Runner.LocalPlayer) return;
            var _id = GetCurrentGameState().newCardId;
            if (_id <= 0) return;
            var _card = CardUtils.FindCardByID(_id);
            CardQueueSystem.AddEnemyCardToQueue(_card);
        }
        
        private void NextGameState()
        {
            if (CurrentStateID < 3)
                CurrentStateID++;
            else
                CurrentStateID = 0;
        }

        #endregion
        
        #region Debug
        private void PrintDebugData()
        {
            int tempGameState = 0;
            foreach (var _state in LastGameStates)
            {
                Debug.LogError("CurrentState ID:" + tempGameState);
                Debug.LogError("New Card ID:" +_state.newCardId);
                Debug.LogError("Player ID:" +_state.GameStatePlayerRef.PlayerId);
                foreach (var cardID in _state.AllCardsThisMatch.ToArray())
                    if(cardID > 0)
                        Debug.Log("Card ID:" + cardID);
                
                tempGameState++;
            }
        }
        #endregion
        
    }

}

public struct GameState : INetworkStruct {

    private const int MAXPLAYERS = 2;
    public PlayerRef GameStatePlayerRef;
    public int newCardId { get; set; }
    [Networked, Capacity(60)] public NetworkArray<Int32> AllCardsThisMatch => default;
    [Networked, Capacity(2)] public NetworkArray<float> PlayersHealth => default;

    //[Networked, Capacity(10)] public NetworkDictionary<Int32, FullToyoStruct> FullToyo => default;

    //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerNetworkStruct> Players => default;
    //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerHandTest> PlayersHand => default;
}

public struct  PlayerInputData : INetworkInput
{
    public int PlayedCardID;
    public PlayerRef PlayerRef;

}

