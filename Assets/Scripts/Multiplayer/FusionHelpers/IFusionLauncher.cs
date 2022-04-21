using System;
using Fusion;

namespace Multiplayer
{
    public interface IFusionLauncher :  INetworkRunnerCallbacks
    {
        Action<NetworkRunner, FusionLauncher.ConnectionStatus, string> OnConnect { get; set; }
        Action<NetworkRunner> OnSpawnWorld { get; set; }
        Action<NetworkRunner, PlayerRef> OnSpawnPlayer { get; set; }
        Action<NetworkRunner, PlayerRef> OnDespawnPlayer { get; set; }
    }
}