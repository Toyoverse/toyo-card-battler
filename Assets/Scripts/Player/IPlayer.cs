using APSystem;
using Fusion;
using HealthSystem;
using PlayerHand;
using ToyoSystem;

namespace Player
{
    public interface IPlayer
    {
        IPlayerHand PlayerHand { get; }
        IFullToyo FullToyo { get; }
        IHealth PlayerHealth { get; }
        IAp PlayerAP { get; }
        PlayerReferences MyPlayerReferences { get; set; }
        PlayerRef NetworkPlayerRef { get; set; }

    }
}