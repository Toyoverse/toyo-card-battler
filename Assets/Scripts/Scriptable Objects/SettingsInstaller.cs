using Scriptable_Objects;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Settings/Game Settings")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{

    public DummyConfigSO dummyModeConfig;
    public CombatConfigSO combatConfig;
    public GlobalCardDataSO globalCardConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(dummyModeConfig).IfNotBound();
        Container.BindInstance(combatConfig).IfNotBound();
        Container.BindInstance(globalCardConfig).IfNotBound();
    }
}
