using System;
using System.Collections.Generic;
using System.Text;
using Card.DeckSystem;
using Fusion;
using Infrastructure;
using Multiplayer;
using Player;
using PlayerHand;
using TMPro;
using ToyoSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Globals
{
	/// <summary>
	/// App entry point and main UI flow management.
	/// </summary>
	public class GameLauncher : MonoBehaviour
	{
		[SerializeField] private PlayerNetworkManager _playerNetworkManagerPrefab;
		[FormerlySerializedAs("_playerPrefab")] [SerializeField] private PlayerNetworkObject playerNetworkObjectPrefab;
		[SerializeField] private TMP_InputField _room;
		[SerializeField] private TextMeshProUGUI _progress;
		[SerializeField] private GameObject _uiGame;
		[SerializeField] private GameObject _cardQueueSystem;


		private FusionLauncher.ConnectionStatus _status = FusionLauncher.ConnectionStatus.Disconnected;
		private GameMode _gameMode = GameMode.Single;
		private NetworkRunner Runner;
		
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
			
			if(!_cardQueueSystem.activeSelf && PlayerNetworkManager.Instance != null && PlayerNetworkManager.Instance.IsWorldReady)
				_cardQueueSystem.SetActive(true);
			
		}

		// What mode to play - Called from the start menu
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
			FusionLauncher launcher = FindObjectOfType<FusionLauncher>();
			if (launcher == null)
				launcher = new GameObject("Launcher").AddComponent<FusionLauncher>();

			LevelManager lm = FindObjectOfType<LevelManager>();
			lm.launcher = launcher;

			//var text = _room.text;
			var text = "test";
			launcher.Launch(_gameMode, text, lm, OnConnectionStatusUpdate, OnSpawnWorld, OnSpawnPlayer, OnDespawnPlayer);
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
			runner.Spawn(_playerNetworkManagerPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);
			void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
			{
				//var _playerNetworkManager = networkObject.gameObject.GetComponent<PlayerNetworkManager>();
			}
			
			
		}

		private void OnSpawnPlayer(NetworkRunner runner, PlayerRef playerref)
		{
			runner.Spawn(playerNetworkObjectPrefab, Vector3.zero, Quaternion.identity, playerref, InitNetworkState);

			if (_gameMode == GameMode.Single)
			{
				runner.Spawn(playerNetworkObjectPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);
			}
			
			void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
			{
				
				var _player = networkObject.gameObject.GetComponent<PlayerNetworkObject>();
				Debug.Log($"Initializing player {playerref.PlayerId}");
				var _fullToyo = DatabaseManager.Instance.GetFullToyoFromFakeID(DatabaseManager.Instance.GetPlayerDatabaseID());
				_player.InitNetworkState(_fullToyo);
				
				//if(playerref.PlayerId == runner.LocalPlayer.PlayerId)
				//	StartCoroutine(FindObjectOfType<PlayerHandUtils>()?.DrawFirstHand(_fullToyo));

			}
				

			SetFirstGameStateDebug();

		}
		
		

		//Todo, replace this with getting all the cards from database
		private void SetFirstGameStateDebug()
		{
			var _deck = FindObjectOfType<Deck>();
			var _allIDS = _deck.AllCardIDS;
			
			PlayerNetworkManager.Instance.SetFirstGameState(_allIDS);
		}


		private void OnDespawnPlayer(NetworkRunner runner, PlayerRef playerref)
		{
			
		}
	}
}