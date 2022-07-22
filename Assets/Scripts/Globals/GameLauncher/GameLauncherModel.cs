﻿using Card.DeckSystem;
using Fusion;
using Infrastructure;
using Multiplayer;
using Player;
using CardSystem.PlayerHand;
using ToyoSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Globals
{
	public class GameLauncherModel : MonoBehaviour
	{
		[SerializeField]
		[FormerlySerializedAs("_playerNetworkManagerPrefab")] private PlayerNetworkManager playerNetworkManagerPrefab;
		[SerializeField]
		[FormerlySerializedAs("_playerPrefab")] private PlayerNetworkObject playerNetworkObjectPrefab;
		[SerializeField]
		[FormerlySerializedAs("_cardQueueSystem")] private GameObject cardQueueSystem;

		private PlayerNetworkManager _playerNetworkManager => GlobalConfig.Instance.PlayerNetworkManager;

		private FusionLauncher.ConnectionStatus _status = FusionLauncher.ConnectionStatus.Disconnected;
		private NetworkRunner _runner;
		private GameMode _gameMode = GameMode.Single;
		
		private void Awake()
		{
			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			OnConnectionStatusUpdate(null, FusionLauncher.ConnectionStatus.Disconnected, "");
		}

		private void Update()
		{
			if(!cardQueueSystem.activeSelf && _playerNetworkManager != null && _playerNetworkManager.IsWorldReady)
				cardQueueSystem.SetActive(true);
		}

		public void OnHostOptions()
		{
			SetGameMode(GameMode.Host);
		}

		public void OnJoinOptions()
		{
			SetGameMode(GameMode.Client);
		}

		public void OnSharedOptions()
		{
			SetGameMode(GameMode.Shared);
		}

		private void SetGameMode(GameMode gamemode)
		{
			_gameMode = gamemode;
		}

		public void OnEnterRoom()
		{
			FusionLauncher _launcher = FindObjectOfType<FusionLauncher>();
			if (_launcher == null)
				_launcher = new GameObject("Launcher").AddComponent<FusionLauncher>();

			LevelManager _lm = FindObjectOfType<LevelManager>();
			_lm.launcher = _launcher;

			//var text = _room.text;
			var _text = "test";
			_launcher.Launch(_gameMode, _text, _lm, OnConnectionStatusUpdate, OnSpawnWorld, OnSpawnPlayer, OnDespawnPlayer);
		}
		
		private void OnConnectionStatusUpdate(NetworkRunner runner, FusionLauncher.ConnectionStatus status, string reason)
		{
			if (!this )
				return;

			if (status != _status)
			{
				switch (status)
				{
					case FusionLauncher.ConnectionStatus.Disconnected:
						Debug.Log("Disconnected!" + reason);
						break;
					case FusionLauncher.ConnectionStatus.Failed:
						Debug.Log("Error!" + reason);
						break;
				}
			}
			_status = status;
		}

		private void OnSpawnWorld(NetworkRunner runner)
		{
			Debug.Log("Spawning GameManager");
			runner.Spawn(playerNetworkManagerPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);
			void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
			{
				//var _playerNetworkManager = networkObject.gameObject.GetComponent<PlayerNetworkManager>();
			}
		}

		private void OnSpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
		{
			runner.Spawn(playerNetworkObjectPrefab, Vector3.zero, Quaternion.identity, playerRef, InitNetworkState);

			if (_gameMode == GameMode.Single)
			{
				runner.Spawn(playerNetworkObjectPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);
			}
			
			void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
			{
				
				var _player = networkObject.gameObject.GetComponent<PlayerNetworkObject>();
				Debug.Log($"Initializing player {playerRef.PlayerId}");
				var _fullToyo = DatabaseManager.Instance.GetFullToyoFromFakeID(DatabaseManager.Instance.GetPlayerDatabaseID());
				_player.InitNetworkState(_fullToyo);

			}
		}

		private void OnDespawnPlayer(NetworkRunner runner, PlayerRef playerRef)
		{
			
		}
	}
}