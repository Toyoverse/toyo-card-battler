using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Fusion;
using Multiplayer;
using UnityEngine;

namespace Player
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        [Networked, Capacity(4)] public NetworkArray<GameState> LastGameStates => default;
        [Networked] public int CurrentStateID { get; set; }

        private static List<PlayerNetworkObject> _players;
        public List<PlayerNetworkObject> Players => _players ??= FindObjectsOfType<PlayerNetworkObject>()?.ToList();

        private static PlayerNetworkManager Instance;

        public static void AddPlayer(PlayerNetworkObject playerNetworkObject) => Instance.Players.Add(playerNetworkObject);
        
        public  static void RemovePlayer(PlayerNetworkObject playerNetworkObject) => Instance.Players.Remove(playerNetworkObject);

        public static PlayerNetworkObject GetPlayer(PlayerRef playerRef) => Instance.Players.FirstOrDefault(_player => _player.NetworkPlayerRef.PlayerId == playerRef.PlayerId);

        private void Awake() => Instance = this;


        #region Host

        public GameState GetCurrentGameState() => GetGameState(CurrentStateID);
        
        private GameState GetGameState(int _currentStateID) => LastGameStates.Get(_currentStateID);
        
        public void SetGameState(PlayerInputData _playerInput)
        {
            NextGameState();
            var state = new GameState
            {
                GameStatePlayerRef = _playerInput.PlayerRef,
                newCardId = _playerInput.PlayedCardID
            };
            LastGameStates.Set(CurrentStateID, state);
            //PrintDebugData();
            
        }

        #endregion

        private void PrintDebugData()
        {
            int tempGameState = 0;
            foreach (var _state in LastGameStates)
            {
                Debug.LogError("CurrentState ID:" + tempGameState);
                Debug.LogError("New Card ID:" +_state.newCardId);
                Debug.LogError("Player ID:" +_state.GameStatePlayerRef.PlayerId);
                tempGameState++;
            }
        }

        private void NextGameState()
        {
            if (CurrentStateID < 3)
                CurrentStateID++;
            else
                CurrentStateID = 0;
        }
        
    }
    
    /*
     * GameState struct Test holds the data for replicating a turn.
     */
    public struct GameState : INetworkStruct {

        private const int MAXPLAYERS = 2;
        public PlayerRef GameStatePlayerRef;
        public NetworkString<_16> newCardId { get; set; }

        //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerNetworkStruct> Players => default;
        //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerHandTest> PlayersHand => default;
    }

}

public struct  PlayerInputData : INetworkInput
{
    public NetworkString<_16> PlayedCardID;
    public PlayerRef PlayerRef;

}

