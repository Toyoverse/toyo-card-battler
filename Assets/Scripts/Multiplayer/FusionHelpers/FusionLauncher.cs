using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using FusionExamples.FusionHelpers;
using Multiplayer;
using UnityEngine;


public class FusionLauncher : MonoBehaviour, IFusionLauncher
{
    public static bool IsConnected => Instance.Runner != null && Instance.Runner.IsCloudReady;
    public static FusionLauncher Instance;
    public static bool IsServer => Instance.Runner.IsServer;
    public static GameMode GameMode => Instance.Runner.GameMode;
    
    private NetworkRunner Runner;
    private ConnectionStatus Status;
    private FusionObjectPoolRoot Pool;

    #region Actions
    
    Action<NetworkRunner, ConnectionStatus, string> IFusionLauncher.OnConnect
    {
        get => OnConnect;
        set => OnConnect = value;
    }
    
    Action<NetworkRunner> IFusionLauncher.OnSpawnWorld
    {
        get => OnSpawnWorld;
        set => OnSpawnWorld = value;
    }
    
    Action<NetworkRunner, PlayerRef> IFusionLauncher.OnSpawnPlayer
    {
        get => OnSpawnPlayer;
        set => OnSpawnPlayer = value;
    }
    
    Action<NetworkRunner, PlayerRef> IFusionLauncher.OnDespawnPlayer
    {
        get => OnDespawnPlayer;
        set => OnDespawnPlayer = value;
    }
    
    private event Action<NetworkRunner> OnSpawnWorld = _ => { };
    private event Action<NetworkRunner, ConnectionStatus, string> OnConnect = (_, _, _) => { };
    private event Action<NetworkRunner, PlayerRef> OnSpawnPlayer = (_, _) => { };
    private event Action<NetworkRunner, PlayerRef> OnDespawnPlayer = (_, _) => { };
    
    #endregion
    
    
    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Failed,
        Connected,
        Loading,
        Loaded
    }

    public async void Launch(GameMode mode, string room,
        INetworkSceneObjectProvider sceneLoader,
        Action<NetworkRunner, ConnectionStatus, string> onConnect,
        Action<NetworkRunner> onSpawnWorld,
        Action<NetworkRunner, PlayerRef> onSpawnPlayer,
        Action<NetworkRunner, PlayerRef> onDespawnPlayer)
    {
        OnConnect = onConnect;
        OnSpawnPlayer = onSpawnPlayer;
        OnSpawnWorld = onSpawnWorld;
        OnDespawnPlayer = onDespawnPlayer;

        SetConnectionStatus(ConnectionStatus.Connecting, "");
        
        //DontDestroyOnLoad(gameObject);

        if (Runner == null)
            Runner = gameObject.AddComponent<NetworkRunner>();
        Runner.name = name;
        Runner.ProvideInput = mode != GameMode.Server;

        if(Pool==null)
            Pool = gameObject.AddComponent<FusionObjectPoolRoot>();
        
        _ = await Runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = room,
            ObjectPool = Pool,
            SceneObjectProvider = sceneLoader
        });

    }




    #region INetworkRunnerCallbacks
    
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
       if (runner.IsServer) InstantiatePlayer(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        OnDespawnPlayer?.Invoke(runner, player);
        SetConnectionStatus(Status, "Player Left");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
        Debug.Log("OnShutdown");
        string message = shutdownReason switch
        {
            /*case GameManager.ShutdownReason_GameAlreadyRunning:
                message = "Game in this room already started!";
                break;*/
            ShutdownReason.IncompatibleConfiguration => "This room already exist in a different game mode!",
            ShutdownReason.Ok => "User terminated network session!",
            ShutdownReason.Error => "Unknown network error!",
            ShutdownReason.ServerInRoom => "There is already a server/host in this room",
            ShutdownReason.DisconnectedByPluginLogic => "The Photon server plugin terminated the network session!",
            _ => shutdownReason.ToString()
        };
        SetConnectionStatus(ConnectionStatus.Disconnected, message);

        // TODO: This cleanup should be handled by the ClearPools call below, but currently Fusion is not returning pooled objects on shutdown, so...
        var _networkObjects = FindObjectsOfType<NetworkObject>();
        for (int i = 0; i < _networkObjects.Length; i++)
            Destroy(_networkObjects[i].gameObject);
			
        Pool.ClearPools();
            
        if(Runner!=null && Runner.gameObject)
            Destroy(Runner.gameObject);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        if (runner.GameMode == GameMode.Shared)
            InstantiatePlayer(runner, runner.LocalPlayer);

        SetConnectionStatus(ConnectionStatus.Connected, "");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        SetConnectionStatus(ConnectionStatus.Disconnected, "disconnected");

        // TODO: This cleanup should be handled by the ClearPools call below, but currently Fusion is not returning pooled objects on shutdown, so...
        // Destroy all NOs
        NetworkObject[] nos = FindObjectsOfType<NetworkObject>();
        for (int i = 0; i < nos.Length; i++)
            Destroy(nos[i].gameObject);
			
        //PlayerManager.ResetPlayerManager();

        // Reset the object pools
        Pool.ClearPools();
            
        if(Runner!=null && Runner.gameObject)
            Destroy(Runner.gameObject);
    
        SetConnectionStatus(ConnectionStatus.Disconnected, "");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        request.Accept();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        SetConnectionStatus(ConnectionStatus.Failed, reason.ToString());
    }
    
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    
    #endregion

    #region Functions

    private void InstantiatePlayer(NetworkRunner runner, PlayerRef playerref)
    {
        if (runner.IsServer || runner.IsSharedModeMasterClient) 
        {
            OnSpawnWorld?.Invoke(runner);
            OnSpawnWorld = null;
        }

        OnSpawnPlayer?.Invoke(runner, playerref);
    }


    public void SetConnectionStatus(ConnectionStatus status, string message)
    {
        Status = status;
        OnConnect?.Invoke(Runner, status, message);
    }

    #endregion

    #region UnityCallbacks

    private void Awake() => Instance = this;

    #endregion
    

}
