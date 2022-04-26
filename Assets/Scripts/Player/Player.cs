using APSystem;
using Fusion;
using HealthSystem;
using PlayerHand;
using ToyoSystem;

namespace Player
{
    public class Player : NetworkBehaviour, IPlayer
    {

        public PlayerReferences MyPlayerReferences { get; set; }
        public PlayerRef NetworkPlayerRef { get; set; }

        public IPlayerHand MyPlayerHand;
        IPlayerHand IPlayer.PlayerHand => MyPlayerHand;
        
        public IFullToyo MyFullToyo;
        IFullToyo IPlayer.FullToyo => MyFullToyo;
        
        public IHealth MyPlayerHealth;
        IHealth IPlayer.PlayerHealth => MyPlayerHealth;
        
        public IAp MyPlayerAP;
        IAp IPlayer.PlayerAP => MyPlayerAP;

        private void Awake()
        {
            MyPlayerReferences = GetComponent<PlayerReferences>();
            MyPlayerHealth = MyPlayerReferences.PlayerUI.GetComponentInChildren<IHealth>();
            MyPlayerAP = MyPlayerReferences.PlayerUI.GetComponentInChildren<IAp>();
            MyFullToyo = MyPlayerReferences.Toyo.GetComponent<IFullToyo>();
            MyPlayerHand = MyPlayerReferences.hand.GetComponent<IPlayerHand>();
            //NetworkPlayerRef = Runner.LocalPlayer;
            //MyPlayerHand.MyPlayerRef = NetworkPlayerRef;
        }
    }
    
    public struct PlayerNetworkStruct : INetworkStruct
    {
        public Player Player;
        public PlayerNetworkStruct(Player _player) => Player = _player;
    }

}