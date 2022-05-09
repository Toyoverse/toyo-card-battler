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

        public override void FixedUpdateNetwork() {
            //The player needs to have InputAuthority over this NetworkObject.
            //You can create a simple NB to read inputs and maybe other infos about each player (All         //player are interested, that will be needed for when you're using AOI)
            //You can have a similar NB for each player like the one in the RazorMadness sample
        
            //Only the InputAuth and the Host will return true and get the input from this check
            if(GetInput(out PlayerInputData data))
            {
                //Seting the state from the Host, the inputAuth will run this as well but no need to             //worry because he cant change the state.
                //But you can do a quick check like if(Object.HasStateAuthority) if you want
                SetGameState(data.newCardId.ToString());
            }
        }
        public GameState GetCurrentGameState() => GetGameState(CurrentStateID);
        
        private GameState GetGameState(int _currentStateID) => LastGameStates.Get(_currentStateID);
        
        public void SetGameState(string cardID)
        {
            var state = new GameState();
            state.CurrentPlayerTurn = Runner.LocalPlayer;
            state.newCardId = cardID;
            LastGameStates.Set(CurrentStateID, state);
            NextGameState();
        }

        #endregion

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
        public PlayerRef CurrentPlayerTurn;
        public NetworkString<_16> newCardId { get; set; }

        //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerNetworkStruct> Players => default;
        //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerHandTest> PlayersHand => default;
    }

}

public struct  PlayerInputData : INetworkInput
{
    public NetworkString<_16> newCardId;

}

