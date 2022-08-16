using System;
using CombatSystem.APSystem;
using Fusion;
using HealthSystem;
using Infrastructure;
using Multiplayer;
using CardSystem.PlayerHand;
using ServiceLocator;
using ToyoSystem;
using UnityEngine;
using Zenject;

namespace Player
{
	[RequireComponent(typeof(NetworkCharacterControllerPrototype))]
    public class PlayerNetworkEntityModel : NetworkBehaviour, IPlayer
    {
	    public enum State
	    {
		    New,
		    DeSpawned,
		    Spawning,
		    Active,
		    Dead
	    }
	    
	    public static PlayerNetworkEntityModel Local { get; set; }
	    public const float MaxHealth = 100.0f;
	    
		public int PlayerID { get; private set; }
		[SerializeField] 
		private float respawnInSeconds = -1;
		public bool IsActivated => (gameObject.activeInHierarchy && (PlayerState == State.Active || PlayerState == State.Spawning));
		public bool IsDead => PlayerState == State.Dead;
		public bool IsEnemy = false;
		
		public PlayerRef NetworkPlayerRef { get; set; }
		[Inject(Id="PlayerHealth")]
		public HealthModel MyPlayerHealthModel { get; set; }
		[Inject(Id="PlayerAP")]
		public ApModel MyPlayerApModel { get; set; }

		private FullToyoSO _fullToyoSo;

	    private State _state;
	    private float _health;

	    private IPlayerHand _myPlayerHand;
	    private IFullToyo _myFullToyo;
	    private NetworkCharacterControllerPrototype _cc;
	    private LevelManager _levelManager;
		private Vector2 _lastMoveDirection; // Store the previous direction for correct hull rotation
		private GameObject _deathExplosionInstance;

		private PlayerNetworkManager _playerNetworkManager;
		private PlayerNetworkManager PlayerNetworkManager => _playerNetworkManager ??= FindObjectOfType<PlayerNetworkManager>();
		
		[Inject]
		public void Construct(IFullToyo fullToyo, IPlayerHand playerHand)
		{
			_myFullToyo = fullToyo;
			_myPlayerHand = playerHand;
		}

		private void Awake()
		{
			_cc = GetComponent<NetworkCharacterControllerPrototype>();
		}

		private void Start()
		{
			MyPlayerHealthModel.InjectPlayer(this);
		}

		public void InitNetworkState(FullToyoSO fullToyoSo)
		{
			PlayerState = State.New;
			_fullToyoSo = fullToyoSo;
			PlayerState = State.Spawning;
		}

		private void Update()
		{
			if (PlayerState != State.Spawning || PlayerHandUtils.IsHandDrawed) return;
			if (!_fullToyoSo)
				_fullToyoSo =
					DatabaseManager.Instance.GetFullToyoFromFakeID(DatabaseManager.Instance.GetPlayerDatabaseID());
			StartCoroutine(FindObjectOfType<PlayerHandUtils>()?.DrawFirstHand(_fullToyoSo));
			PlayerState = State.Active;
		}

		public override void Spawned()
		{
			if (Object.HasInputAuthority)
				Local = this;

			// Getting this here because it will revert to -1 if the player disconnects, but we still want to remember the Id we were assigned for clean-up purposes
			PlayerID = Object.InputAuthority;

			NetworkPlayerRef = Runner.LocalPlayer;
			_myPlayerHand.MyPlayerRef = NetworkPlayerRef;

			PlayerNetworkManager.AddPlayer(this);

			InitializeHealth();

			// Auto will set proxies to InterpolationDataSources.Snapshots and State/Input authority to InterpolationDataSources.Predicted
			// The NCC must use snapshots on proxies for lag compensated raycasts to work properly against them.
			// The benefit of "Auto" is that it will update automatically if InputAuthority is changed (this is not relevant in this game, but worth keeping in mind)
			GetComponent<NetworkCharacterControllerPrototype>().InterpolationDataSource = InterpolationDataSources.Auto;
		}
		
		[Inject]
		public void Construct(IPlayerHand playerHand)
		{
			_myPlayerHand = playerHand;
		}

		private void InitializeHealth()
		{
			
			if (PlayerID == Runner.LocalPlayer.PlayerId)
			{
				var playerUI = PlayerNetworkManager.PlayerUI;
				playerUI.SetActive(true);
				MyPlayerHealthModel = playerUI.GetComponentInChildren<HealthModel>();
				MyPlayerHealthModel.Parent = this;
			}
			else
			{
				var enemyUI = PlayerNetworkManager.EnemyUI;
				enemyUI.SetActive(true);
				MyPlayerHealthModel = enemyUI.GetComponentInChildren<HealthModel>();
				IsEnemy = true;
				MyPlayerHealthModel.Parent = this;
			}

			if (FusionLauncher.GameMode == GameMode.Single)
			{
				var enemyUI = PlayerNetworkManager.EnemyUI;
				enemyUI.SetActive(true);
			}

			Health = MaxHealth;
		}

		public override void FixedUpdateNetwork()
		{
			
		}

		/// <summary>
		/// Render is the Fusion equivalent of Unity's Update() and unlike FixedUpdateNetwork which is very different from FixedUpdate,
		/// Render is in fact exactly the same. It even uses the same Time.deltaTime time steps. The purpose of Render is that
		/// it is always called *after* FixedUpdateNetwork - so to be safe you should use Render over Update if you're on a
		/// SimulationBehaviour.
		/// </summary>
		public override void Render() { }

		
		public static void OnStateChanged(Changed<PlayerNetworkEntityModel> changed)
		{
			if(changed.Behaviour)
				changed.Behaviour.OnStateChanged();
		}
		
		public void OnStateChanged()
		{
			switch (PlayerState)
			{
				case State.Spawning:
					//_teleportIn.StartTeleport();
					break;
				case State.Active:
					//_damageVisuals.CleanUpDebris();
					//_teleportIn.EndTeleport();
					break;
				case State.Dead:
					/*_deathExplosionInstance.transform.position = transform.position;
					_deathExplosionInstance.SetActive(false); // dirty fix to reactivate the death explosion if the particlesystem is still active
					_deathExplosionInstance.SetActive(true);

					_visualParent.gameObject.SetActive(false);
					_damageVisuals.OnDeath();*/
					break;
				case State.DeSpawned:
					//_teleportOut.StartTeleport();
					break;
			}
		}
		
		public static void OnHealthChanged(Changed<PlayerNetworkEntityModel> changed)
		{
			if(changed.Behaviour)
				changed.Behaviour.OnHealthChanged();
		}

		public void OnHealthChanged()
		{
			MyPlayerHealthModel?.OnChangeHp?.Invoke(Health);
		}

		public void OnComboChanged()
		{
			Debug.LogError("Behaviour.OnComboChanged");
		}

		private void ResetPlayer()
		{
			Debug.Log($"Resetting player {PlayerID}, tick={Runner.Simulation.Tick}, life={Health}, hasAuthority={Object.HasStateAuthority} to state={PlayerState}");
			//shooter.ResetAllWeapons();
			PlayerState = State.Active;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			Destroy(_deathExplosionInstance);
			PlayerNetworkManager.RemovePlayer(this);
		}

		#region Getters/Setters

		[Networked(OnChanged = nameof(OnStateChanged))]
		public State PlayerState
		{
			get => _state;
			set => _state = value;
		}

		[Networked(OnChanged = nameof(OnHealthChanged))]
		public float Health
		{
			get => _health;
			set => _health = value;
		}
		
		IPlayerHand IPlayer.PlayerHand => _myPlayerHand;
		IFullToyo IPlayer.FullToyo => _myFullToyo;
		HealthModel IPlayer.PlayerHealthModel => MyPlayerHealthModel;
		ApModel IPlayer.PlayerApModel => MyPlayerApModel;
		
		#endregion
    }
}