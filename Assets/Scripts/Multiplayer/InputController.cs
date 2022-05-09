using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Fusion;
using Fusion.Sockets;
using Player;
using PlayerHand;
using UnityEngine;
using Extensions;


/// <summary>
/// Handle player input by responding to Fusion input polling, filling an input struct and then working with
/// that input struct in the Fusion Simulation loop.
/// </summary>
public class InputController : NetworkBehaviour, INetworkRunnerCallbacks
{
	private string IDCardForQueue { get; set; }

	private int PreviousStateID { get; set; }

	private List<string> _listCardIDsInQueue = new ();
	public List<ICard> ListCardIDsInQueue => _listCardIDsInQueue.Select(CardUtils.FindCardByID).ToList();

	public static bool fetchInput = true;
	public bool ToggleReady { get; set; }

	private Vector2 _moveDelta;
	private Vector2 _aimDelta;
	private Vector2 _leftPos;
	private Vector2 _leftDown;
	private Vector2 _rightPos;
	private Vector2 _rightDown;
	private bool _leftTouchWasDown;
	private bool _rightTouchWasDown;
	private bool _primaryFire;
	private bool _secondaryFire;


	
	private PlayerInputData _frameworkInput = new PlayerInputData();

	#region LazyProperties
	
	//Lazy FindObject
	private Lazy<PlayerNetworkManager> _playerNetworkManager = new (FindObjectOfType<PlayerNetworkManager>);
	public PlayerNetworkManager PlayerNetworkManager => _playerNetworkManager.Value;
	
	//Cached Property GetComponent
	private PlayerNetworkObject _playerNetworkObject;
	public PlayerNetworkObject PlayerNetworkObject => this.LazyGetComponent(ref _playerNetworkObject);

	//Cached Property FindInterface
	private IPlayerHand _cardHand;
	public IPlayerHand CardHand => _cardHand ??= this.LazyFindOfType(ref _cardHand);

	#endregion

	//private MobileInput _mobileInput;

	/// <summary>
	/// Hook up to the Fusion callbacks so we can handle the input polling
	/// </summary>
	public override void Spawned()
	{
		// Technically, it does not really matter which InputController fills the input structure, since the actual data will only be sent to the one that does have authority,
		// but in the name of clarity, let's make sure we give input control to the gameobject that also has Input authority.
		if (Object.HasInputAuthority)
		{
			Runner.AddCallbacks(this);
		}

		//Debug.Log("Spawned [" + this + "] IsClient=" + Runner.IsClient + " IsServer=" + Runner.IsServer + " HasInputAuth=" + Object.HasInputAuthority + " HasStateAuth=" + Object.HasStateAuthority);
	}



	void AddCardToQueue(ICard _card) => IDCardForQueue = _card.ID;
	
	private void OnEnable()
	{
		CardHand.OnCardPlayed += AddCardToQueue;
		/*
		if (!FusionLauncher.IsConnected) return;
		var myNetworkRunner = FindObjectOfType<NetworkRunner>();
		myNetworkRunner.AddCallbacks(this);
		*/
	}
	
	public void OnDisable()
	{
		CardHand.OnCardPlayed -= AddCardToQueue;
		if (FusionLauncher.IsConnected == false) return;
		var myNetworkRunner = FindObjectOfType<NetworkRunner>();
		if (myNetworkRunner == null)
			return;
		myNetworkRunner.RemoveCallbacks(this);
	}


	/// <summary>
	/// Get Unity input and store them in a struct for Fusion
	/// </summary>
	/// <param name="runner">The current NetworkRunner</param>
	/// <param name="input">The target input handler that we'll pass our data to</param>
	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
		if (!string.IsNullOrEmpty(IDCardForQueue))
		{
			SetPlayerInput();
				
			//Hand over data to Fusion
			input.Set(_frameworkInput);
			IDCardForQueue = "";
		}
	}

	void SetPlayerInput()
	{
		_listCardIDsInQueue.Add(IDCardForQueue);
		_frameworkInput = new PlayerInputData
		{
			PlayedCardID = IDCardForQueue,
			PlayerRef = Runner.LocalPlayer
		};
	}

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	/// <summary>
	/// FixedUpdateNetwork is the main Fusion simulation callback - this is where
	/// we modify network state.
	/// </summary>
	public override void FixedUpdateNetwork()
	{
		//The player needs to have InputAuthority over this NetworkObject.
		//You can create a simple NB to read inputs and maybe other infos about each player (All
		////player are interested, that will be needed for when you're using AOI)
		//You can have a similar NB for each player like the one in the RazorMadness sample
        
		//Only the InputAuth and the Host will return true and get the input from this check
		if(GetInput(out PlayerInputData data))
		{
			//Seting the state from the Host, the inputAuth will run this as well but no need to
			////worry because he cant change the state.
			//But you can do a quick check like if(Object.HasStateAuthority) if you want
			PlayerNetworkManager.SetGameState(data);
		}

		if (PreviousStateID != PlayerNetworkManager.CurrentStateID && !Object.HasStateAuthority)
		{
			//Todo Running 2 times, once for each player... fix that.
			
			PreviousStateID = PlayerNetworkManager.CurrentStateID;
			GameState _state = PlayerNetworkManager.GetCurrentGameState();
			
			/*
			Debug.Log(_state.newCardId);
			Debug.Log(_state.GameStatePlayerRef.PlayerId);
			*/
		}

	}

	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
	public void OnConnectedToServer(NetworkRunner runner) { }
	public void OnDisconnectedFromServer(NetworkRunner runner) { }
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
	public void OnSceneLoadDone(NetworkRunner runner) { }
	public void OnSceneLoadStart(NetworkRunner runner) { }




}
