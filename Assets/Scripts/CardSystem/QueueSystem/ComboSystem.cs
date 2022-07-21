using Player;
using Scriptable_Objects;
using Zenject;

namespace Card.QueueSystem
{
    public class ComboSystem 
    {
        private int PlayerCurrentCombo { get; set; }
        private int EnemyCurrentCombo { get; set; }
        public int GetOfflinePlayerCurrentCombo() => PlayerCurrentCombo;
        public int GetOfflineEnemyCurrentCombo() => EnemyCurrentCombo;
        public int GetNetworkedPlayerCurrentCombo() => PlayerNetworkManager.Instance.PlayerCurrentCombo;
        public int GetNetworkedEnemyCurrentCombo() => PlayerNetworkManager.Instance.EnemyCurrentCombo;

        [Inject]
        private CombatConfigSO _combatConfig;

        private static bool IsEnemy = false;

        private void AddPlayerCombo() => PlayerCurrentCombo += _combatConfig.comboSum;
        private void AddEnemyCombo() => EnemyCurrentCombo += _combatConfig.comboSum;

        public void ComboBreak()
        {
            PlayerCurrentCombo *= _combatConfig.comboBreakMultiplier;
            EnemyCurrentCombo *= _combatConfig.comboBreakMultiplier;
        }

        public void ComboPlus(bool _isEnemy)
        {
            if(ComboSystem.IsEnemy != _isEnemy)
                ComboBreak();
            ComboSystem.IsEnemy = _isEnemy;
            if(ComboSystem.IsEnemy)
                AddEnemyCombo();
            else
                AddPlayerCombo();
        }
    }
}