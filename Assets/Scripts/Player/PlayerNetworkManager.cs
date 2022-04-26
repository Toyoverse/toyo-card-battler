using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Multiplayer;

namespace Player
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        [Networked, Capacity(4)] public NetworkArray<GameState> LastGameStates => default;
        [Networked] public int CurrentStateID { get; set; }

        private static List<Player> _players;
        public static List<Player> Players => _players;

        private IFusionLauncher Launcher;

        void Awake()
        {
            _players = FindObjectsOfType<Player>()?.ToList();
            Launcher = GetComponent<IFusionLauncher>();
        }

        static void AddPlayer(Player player) => _players.Add(player);
        
        static void RemovePlayer(Player player) => _players.Remove(player);

        public static Player GetPlayer(PlayerRef playerRef)
        {
            foreach (var _player in _players)
            {
                if (_player.NetworkPlayerRef.PlayerId == playerRef.PlayerId)
                    return _player;
            }

            return null;
        }

        #region Host

        public override void Spawned()
        {
            // Doing on spawned just for simplicity
            if (Object.HasStateAuthority)
                SetGameState();
        }

        public void SetGameState()
        {
            var state = new GameState();
            state.CurrentPlayerTurn = Runner.LocalPlayer;

            foreach (var _player in _players)
                state.Players.Add(_player.NetworkPlayerRef, new PlayerNetworkStruct(_player));
                
            //state.PlayersHealth.Add(Runner.LocalPlayer, 100);
            //state.PlayersHand.Add(Runner.LocalPlayer, PlayerHandTest.SampleHand);
            /*
            state.TurnAction = new TurnAction();
            state.TurnAction.SourceCardId = 10;
            state.TurnAction.TargetCardId = 15;
            state.TurnAction.Move = TurnMove.Attack;
            */
            LastGameStates.Set(CurrentStateID, state);
        }

        #endregion

        
        
    }
    
    /*
     * GameState struct Test holds the data for replicating a turn.
     */
    public struct GameState : INetworkStruct {

        private const int MAXPLAYERS = 2;
        public PlayerRef CurrentPlayerTurn;
        
        [Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerNetworkStruct> Players => default;
        
        
        
        //[Networked, Capacity(MAXPLAYERS)] public NetworkDictionary<PlayerRef, PlayerHandTest> PlayersHand => default;
    }

    
    /*

    public struct PlayerHandTest : INetworkStruct {
        [Networked, Capacity(10)] public NetworkArray<int> Cards => default;

        public static PlayerHandTest SampleHand
        {
            get {
                var hand = new PlayerHandTest();
                hand.Cards.Set(0, 10);
                hand.Cards.Set(1, 15);
                hand.Cards.Set(2, 13);
                return hand;
            }
        }
    }

    public enum TurnMove{Attack, Buff}*/
    
    
}

