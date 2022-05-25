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

namespace FusionExamples.Tanknarok
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
			//Debug.LogError(_gameMode);
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
			UpdateUI();
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
			/*
			Debug.Log($"Despawning Player {playerref}");
			Player player = PlayerManager.Get(playerref);
			player.TriggerDespawn();
			*/
		}
		
	
		/*private void OnGUI()
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
		}*/


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


/* BKP JSON Compression
				List<byte[]> _toyoPartsJSON = new();
				foreach (var _toyoPart in _fullToyo.ToyoParts)
				{
					var _json = _toyoPart.DumpJson();
					var _bytes = Encoding.ASCII.GetBytes(_json);
					var _compressedJson = LZMAtools.CompressByteArrayToLZMAByteArray(_bytes);
					_toyoPartsJSON.Add(_compressedJson);
					Debug.Log(_compressedJson);
				}
				
				SetFirstGameState(_toyoPartsJSON);
				
				
				byte[] text1 = Encoding.ASCII.GetBytes(new string ('X', 10000));
				byte[] compressed = LZMAtools.CompressByteArrayToLZMAByteArray(text1);
				byte[] text2 = LZMAtools.DecompressLZMAByteArrayToByteArray(compressed);
     
				string longstring = "defined input is deluciously delicious.14 And here and Nora called The reversal from ground from here and executed with touch the country road, Nora made of, reliance on, can’t publish the goals of grandeur, said to his book and encouraging an envelope, and enable entry into the chryssial shimmering of hers, so God of information in her hands Spiros sits down the sign of winter? —It’s kind of Spice Christ. It is one hundred birds circle above the text: They did we said. 69 percent dead. Sissy Cogan’s shadow. —Are you x then sings.) I’m 96 percent dead humanoid figure,";
				byte[] text3 = Encoding.ASCII.GetBytes(longstring);
				byte[] compressed2 = LZMAtools.CompressByteArrayToLZMAByteArray(text3);
				byte[] text4 = LZMAtools.DecompressLZMAByteArrayToByteArray(compressed2);
   */

