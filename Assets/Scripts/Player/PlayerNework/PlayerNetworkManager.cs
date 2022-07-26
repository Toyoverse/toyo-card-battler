using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Card.DeckSystem;
using Card.QueueSystem;
using Fusion;
using HealthSystem;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        [Networked]
        public bool IsWorldReady { get; set; }
        public GameObject PlayerUI => GlobalConfig.Instance.PlayerUI;
        public GameObject EnemyUI =>  GlobalConfig.Instance.EnemyUI;
        [Networked, Capacity(4)] public NetworkArray<GameState> LastGameStates => default;
        [Networked] public int CurrentStateID { get; set; }
        public List<PlayerNetworkEntityModel> Players => _players ??= FindObjectsOfType<PlayerNetworkEntityModel>()?.ToList();
        
        private HealthModel _playerHealthModel;
        private HealthModel _enemyHealthModel;
        private static List<PlayerNetworkEntityModel> _players;
        
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Fire<PlayerNetworkInitializedSignal>(new() { PlayerNetworkManager = this });
            SetFirstGameState(FindObjectOfType<Deck>()._allCardsID);
        }

        #region Statics
        
        public void AddPlayer(PlayerNetworkEntityModel playerNetworkEntityModel) => Players.Add(playerNetworkEntityModel);
        
        public void RemovePlayer(PlayerNetworkEntityModel playerNetworkEntityModel) => Players.Remove(playerNetworkEntityModel);

        public PlayerNetworkEntityModel GetPlayer(PlayerRef playerRef) => Players.FirstOrDefault(player => player.NetworkPlayerRef.PlayerId == playerRef.PlayerId);
        
        public PlayerNetworkEntityModel GetLocalPlayer() => Players.FirstOrDefault(player => !player.IsEnemy);
        
        public PlayerNetworkEntityModel GetEnemy() => Players.FirstOrDefault(player => player.IsEnemy);

        public List<ICard> GetCardQueuePlayer() => PlayerCardQueue;
        
        public List<ICard> GetCardQueueEnemy() => EnemyCardQueue;
        
        #endregion

        #region Host

        public GameState GetCurrentGameState() => GetGameState(CurrentStateID);
        
        private GameState GetGameState(int currentStateID) => LastGameStates.Get(currentStateID);

        public CardQueueSystemModel CardQueueSystemModel => _cardQueueSystemModel ??= FindObjectOfType<CardQueueSystemModel>();
        private CardQueueSystemModel _cardQueueSystemModel;

        public List<ICard> AllCards = new();
        private List<int> _allCardIds = new();

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
            if (!Object.HasStateAuthority) return;
            if (CardQueueSystemModel != null)
            {
                CurrentCardDuration = CardQueueSystemModel.CurrentCardDuration;
                PlayerCurrentCombo = CardQueueSystemModel.ComboSystem.GetOfflinePlayerCurrentCombo();
                EnemyCurrentCombo = CardQueueSystemModel.ComboSystem.GetOfflineEnemyCurrentCombo();
            }

            PlayerQueueSize = PlayerCardQueue.Count;
            EnemyQueueSize = EnemyCardQueue.Count;
            if(!IsWorldReady)
                IsWorldReady = true;
        }


        /*
         * Todo: VERY BAD -- Temporary - Changing for unity adressables next sprint
         */
        public void SetFirstGameState(List<int> cardIds)
        {
            _allCardIds.AddRange(cardIds);
            AllCards.AddRange(CardUtils.FindCardsByIDs(cardIds));
            
            var state = new GameState();
            for (int i = 0; i < _allCardIds.Count; i++)
                state.AllCardsThisMatch.Set(i,_allCardIds[i]);
            
            NextGameState();
            LastGameStates.Set(CurrentStateID, state);
        }
        
        public void SetGameState(PlayerInputData playerInput)
        {
            NextGameState();
            var state = new GameState
            {
                GameStatePlayerRef = playerInput.PlayerRef,
                NewCardId = playerInput.PlayedCardID
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
            var id = GetCurrentGameState().NewCardId;
            if (id <= 0) return;
            var card = CardUtils.FindCardByID(id);
            CardQueueSystemModel.AddEnemyCardToQueue(card);
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
            foreach (var state in LastGameStates)
            {
                Debug.LogError("CurrentState ID:" + tempGameState);
                Debug.LogError("New Card ID:" +state.NewCardId);
                Debug.LogError("Player ID:" +state.GameStatePlayerRef.PlayerId);
                foreach (var cardID in state.AllCardsThisMatch.ToArray())
                    if(cardID > 0)
                        Debug.Log("Card ID:" + cardID);
                
                tempGameState++;
            }
        }
        #endregion
        
    }
    
    public class PlayerNetworkInitializedSignal
    {
        public PlayerNetworkManager PlayerNetworkManager;
    }

}

public struct GameState : INetworkStruct {

    private const int MaxPlayers = 2;
    public PlayerRef GameStatePlayerRef;
    public int NewCardId { get; set; }
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

