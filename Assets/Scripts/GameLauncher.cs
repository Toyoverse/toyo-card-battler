using Fusion;
using Multiplayer;
using Player;
using TMPro;
using UnityEngine;

namespace FusionExamples.Tanknarok
{
	/// <summary>
	/// App entry point and main UI flow management.
	/// </summary>
	public class GameLauncher : MonoBehaviour
	{
		[SerializeField] private PlayerNetworkManager _playerNetworkManagerPrefab;
		[SerializeField] private Player.Player _playerPrefab;
		[SerializeField] private TMP_InputField _room;
		[SerializeField] private TextMeshProUGUI _progress;
		/*[SerializeField] private Panel _uiCurtain;
		[SerializeField] private Panel _uiStart;
		[SerializeField] private Panel _uiProgress;
		[SerializeField] private Panel _uiRoom;
		*/
		[SerializeField] private GameObject _uiGame;

		private FusionLauncher.ConnectionStatus _status = FusionLauncher.ConnectionStatus.Disconnected;
		private GameMode _gameMode;
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
			/*
			if (_uiProgress.isShowing)
			{
				if (Input.GetKeyUp(KeyCode.Escape))
				{
					NetworkRunner runner = FindObjectOfType<NetworkRunner>();
					if (runner != null && !runner.IsShutdown)
					{
						// Calling with destroyGameObject false because we do this in the OnShutdown callback on FusionLauncher
						runner.Shutdown(false);
					}
				}
				UpdateUI();
			}
			*/
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
			/*if (GateUI(_uiStart))
				_uiRoom.SetVisible(true);
			*/
		}

		public void OnEnterRoom()
		{
			//if (GateUI(_uiRoom))
			//{
		    FusionLauncher launcher = FindObjectOfType<FusionLauncher>();
				if (launcher == null)
					launcher = new GameObject("Launcher").AddComponent<FusionLauncher>();

				LevelManager lm = FindObjectOfType<LevelManager>();
				lm.launcher = launcher;

				//var text = _room.text;
				var text = "test";
				launcher.Launch(_gameMode, text, lm, OnConnectionStatusUpdate, OnSpawnWorld, OnSpawnPlayer, OnDespawnPlayer);
			//}
		}

		/// <summary>
		/// Call this method from button events to close the current UI panel and check the return value to decide
		/// if it's ok to proceed with handling the button events. Prevents double-actions and makes sure UI panels are closed. 
		/// </summary>
		/// <param name="ui">Currently visible UI that should be closed</param>
		/// <returns>True if UI is in fact visible and action should proceed</returns>
		/*private bool GateUI(Panel ui)
		{
			if (!ui.isShowing)
				return false;
			ui.SetVisible(false);
			return true;
		}
		*/
		private void OnConnectionStatusUpdate(NetworkRunner runner, FusionLauncher.ConnectionStatus status, string reason)
		{
			if (!this)
				return;

			Debug.Log(status);

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
			UpdateUI();
		}

		private void OnSpawnWorld(NetworkRunner runner)
		{
			Debug.Log("Spawning GameManager");
			runner.Spawn(_playerNetworkManagerPrefab, Vector3.zero, Quaternion.identity, null, InitNetworkState);
			void InitNetworkState(NetworkRunner runner, NetworkObject world)
			{
				world.transform.parent = transform;
			}
		}

		private void OnSpawnPlayer(NetworkRunner runner, PlayerRef playerref)
		{
			//Debug.Log(playerref.PlayerId);

			/*
			if (GameManager.playState != GameManager.PlayState.LOBBY)
			{
				Debug.Log("Not Spawning Player - game has already started");
				return;
			}
			Debug.Log($"Spawning tank for player {playerref}");
			runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, playerref, InitNetworkState);
			void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
			{
				Player player = networkObject.gameObject.GetComponent<Player>();
				Debug.Log($"Initializing player {player.playerID}");
				player.InitNetworkState(GameManager.MAX_LIVES);
			}
			*/
			runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, playerref, InitNetworkState);
			void InitNetworkState(NetworkRunner runner, NetworkObject networkObject)
			{
				var _player = networkObject.gameObject.GetComponent<Player.Player>();
				Debug.Log($"Initializing player {playerref.PlayerId}");
				//player.InitNetworkState(GameManager.MAX_LIVES);
			}
			/*
			//var _player = PlayerNetworkManager.GetPlayer(playerref);
			_player.Runner = runner;
			//Runner = runner;
			_player.NetworkPlayerRef = runner.LocalPlayer;
			_player.MyPlayerHand.MyPlayerRef = _player.NetworkPlayerRef;*/
		}

		private void OnDespawnPlayer(NetworkRunner runner, PlayerRef playerref)
		{
			/*
			Debug.Log($"Despawning Player {playerref}");
			Player player = PlayerManager.Get(playerref);
			player.TriggerDespawn();
			*/
		}
		
	
		private void OnGUI()
		{
			if (Runner == null)
			{
				if (GUI.Button(new Rect(0,0,200,40), "Host"))
				{
					SetGameMode(GameMode.Host);
					OnEnterRoom();
				}
				if (GUI.Button(new Rect(0,40,200,40), "Join"))
				{
					SetGameMode(GameMode.Client);
					OnEnterRoom();
				}
				if (GUI.Button(new Rect(0,80,200,40), "Shared"))
				{
					SetGameMode(GameMode.Shared);
					OnEnterRoom();
				}
				
			}
		}

		private void UpdateUI()
		{
			return;
			bool intro = false;
			bool progress = false;
			bool running = false;

			switch (_status)
			{
				case FusionLauncher.ConnectionStatus.Disconnected:
					_progress.text = "Disconnected!";
					intro = true;
					break;
				case FusionLauncher.ConnectionStatus.Failed:
					_progress.text = "Failed!";
					intro = true;
					break;
				case FusionLauncher.ConnectionStatus.Connecting:
					_progress.text = "Connecting";
					progress = true;
					break;
				case FusionLauncher.ConnectionStatus.Connected:
					_progress.text = "Connected";
					progress = true;
					break;
				case FusionLauncher.ConnectionStatus.Loading:
					_progress.text = "Loading";
					progress = true;
					break;
				case FusionLauncher.ConnectionStatus.Loaded:
					running = true;
					break;
			}

			/*
			_uiCurtain.SetVisible(!running);
			_uiStart.SetVisible(intro);
			_uiProgress.SetVisible(progress);
			*/
			_uiGame.SetActive(running);
			
		}
	}
}