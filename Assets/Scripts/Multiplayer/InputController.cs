using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Fusion;
using Fusion.Sockets;
using Player;
using PlayerHand;
using Tools.Extensions;


/// <summary>
/// Handle player input by responding to Fusion input polling, filling an input struct and then working with
/// that input struct in the Fusion Simulation loop.
/// </summary>
public class InputController : NetworkBehaviour, INetworkRunnerCallbacks
{
	private int IDCardForQueue { get; set; }

	private List<int> _listCardIDsInQueue = new ();
	public List<ICard> ListCardIDsInQueue => _listCardIDsInQueue.Select(CardUtils.FindCardByID).ToList();


	private PlayerInputData _frameworkInput = new ();

	#region LazyProperties
	
	//Lazy FindObject
	private Lazy<PlayerNetworkManager> _playerNetworkManager = new (FindObjectOfType<PlayerNetworkManager>);
	public PlayerNetworkManager PlayerNetworkManager => _playerNetworkManager.Value;
	
	//Cached Property GetComponent
	private PlayerNetworkEntityModel _playerNetworkEntityModel;
	public PlayerNetworkEntityModel PlayerNetworkEntityModel => this.LazyGetComponent(ref _playerNetworkEntityModel);

	//Cached Property FindInterface
	private IPlayerHand _cardHand;
	public IPlayerHand CardHand => _cardHand ??= this.LazyFindOfType(ref _cardHand);

	#endregion

	/// <summary>
	/// Hook up to the Fusion callbacks so we can handle the input polling
	/// </summary>
	public override void Spawned()
	{
		if (Object.HasInputAuthority)
		{
			Runner.AddCallbacks(this);
		}
	}



	void AddCardToQueue(ICard _card) => IDCardForQueue = _card.CardID;
	
	private void OnEnable()
	{
		CardHand.OnAddCardToQueue += AddCardToQueue;
	}
	
	public void OnDisable()
	{
		CardHand.OnAddCardToQueue -= AddCardToQueue;
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
		if (IDCardForQueue <= 0) return;
		SetPlayerInput();
				
		//Hand over data to Fusion
		input.Set(_frameworkInput);
		IDCardForQueue = 0;
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
