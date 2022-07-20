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
        HealthModel PlayerHealthModel { get; }
        ApModel PlayerApModel { get; }
        BattleReferences MyBattleReferences { get; set; }
        PlayerRef NetworkPlayerRef { get; set; }

    }
}