using CombatSystem.APSystem;
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
        IHealthModel PlayerHealthModel { get; }
        IApModel PlayerApModel { get; }
        BattleReferences MyBattleReferences { get; set; }
        PlayerRef NetworkPlayerRef { get; set; }

    }
}