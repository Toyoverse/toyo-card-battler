using Player;

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

        private static bool IsEnemy = false;

        private void AddPlayerCombo() => PlayerCurrentCombo += GlobalConfig.Instance.CombatConfigSO.comboSum;
        private void AddEnemyCombo() => EnemyCurrentCombo += GlobalConfig.Instance.CombatConfigSO.comboSum;

        public void ComboBreak()
        {
            PlayerCurrentCombo *= GlobalConfig.Instance.CombatConfigSO.comboBreakMultiplier;
            EnemyCurrentCombo *= GlobalConfig.Instance.CombatConfigSO.comboBreakMultiplier;
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