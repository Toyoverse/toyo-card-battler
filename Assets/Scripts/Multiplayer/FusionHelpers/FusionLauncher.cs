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
        
        DontDestroyOnLoad(gameObject);

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
        //Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)*3,1,0);
        //NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        //_spawnedCharacters.Add(player, networkPlayerObject);

       if (runner.IsServer) InstantiatePlayer(runner, player);
        
        //Debug.Log("Spawning Player: " + player.PlayerId);
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        /*       if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }*/

        //Debug.Log("Player Left");
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
        //Debug.Log("Connected to server");
        if (runner.GameMode == GameMode.Shared)
        {
            //Debug.Log("Shared Mode - Spawning Player");
            InstantiatePlayer(runner, runner.LocalPlayer);
        }
        SetConnectionStatus(ConnectionStatus.Connected, "");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        //Debug.Log("Disconnected from server");
        SetConnectionStatus(ConnectionStatus.Disconnected, "");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        request.Accept();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //Debug.Log($"Connect failed {reason}");
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
