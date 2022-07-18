using CombatSystem.APSystem;
using Fusion;
using HealthSystem;
using Infrastructure;
using Multiplayer;
using PlayerHand;
using ToyoSystem;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(NetworkCharacterControllerPrototype))]
    public class PlayerNetworkObject : NetworkBehaviour, IPlayer
    {
	    public const float MAX_HEALTH = 100.0f;
		[Networked(OnChanged = nameof(OnStateChanged))]
		public State state { get; set; }

		[Networked(OnChanged = nameof(OnHealthChanged))]
		public float Health { get; set; }
		
		
		public BattleReferences MyBattleReferences { get; set; }
		public PlayerRef NetworkPlayerRef { get; set; }

		public IPlayerHand MyPlayerHand;
		public IFullToyo MyFullToyo;
		public IHealthModel MyPlayerHealthModel;
		public IApModel MyPlayerApModel;
		
		//[Networked]
		IPlayerHand IPlayer.PlayerHand => MyPlayerHand;
        
		//[Networked]
		IFullToyo IPlayer.FullToyo => MyFullToyo;
		
		//[Networked]
		IHealthModel IPlayer.PlayerHealthModel => MyPlayerHealthModel;
        
		//[Networked]
		IApModel IPlayer.PlayerApModel => MyPlayerApModel;

		public FullToyoSO FullToyoSo; 

		
		public static PlayerNetworkObject local { get; set; }

		public enum State
		{
			New,
			Despawned,
			Spawning,
			Active,
			Dead
		}

		public bool isActivated => (gameObject.activeInHierarchy && (state == State.Active || state == State.Spawning));
		public bool isDead => state == State.Dead;
		public bool IsEnemy = false;

		public int playerID { get; private set; }


		private NetworkCharacterControllerPrototype _cc;

		private LevelManager _levelManager;
		private Vector2 _lastMoveDirection; // Store the previous direction for correct hull rotation
		private GameObject _deathExplosionInstance;
		private float _respawnInSeconds = -1;

		private void Awake()
		{
			_cc = GetComponent<NetworkCharacterControllerPrototype>();
			MyBattleReferences = FindObjectOfType<BattleReferences>();
			MyPlayerApModel = MyBattleReferences.PlayerUI.GetComponentInChildren<IApModel>();
			MyFullToyo = MyBattleReferences.Toyo.GetComponent<IFullToyo>();
			MyPlayerHand = MyBattleReferences.hand.GetComponent<IPlayerHand>();
		}


		private LevelManager GetLevelManager()
		{
			if (_levelManager == null)
				_levelManager = FindObjectOfType<LevelManager>();
			return _levelManager;
		}
		
		public void InitNetworkState(FullToyoSO fullToyoSO)
		{
			state = State.New;
			FullToyoSo = fullToyoSO;
			state = State.Spawning;
		}

		private void Update()
		{
			if (state == State.Spawning && !PlayerHandUtils.IsHandDrawed)
			{
				if (!FullToyoSo)
					FullToyoSo = DatabaseManager.Instance.GetFullToyoFromFakeID(DatabaseManager.Instance.GetPlayerDatabaseID());
				StartCoroutine(FindObjectOfType<PlayerHandUtils>()?.DrawFirstHand(FullToyoSo));
				state = State.Active;
			}
			
		}

		public override void Spawned()
		{
			if (Object.HasInputAuthority)
				local = this;

			// Getting this here because it will revert to -1 if the player disconnects, but we still want to remember the Id we were assigned for clean-up purposes
			playerID = Object.InputAuthority;

			NetworkPlayerRef = Runner.LocalPlayer;
			MyPlayerHand.MyPlayerRef = NetworkPlayerRef;

			PlayerNetworkManager.AddPlayer(this);

			InitializeHealth();

			// Auto will set proxies to InterpolationDataSources.Snapshots and State/Input authority to InterpolationDataSources.Predicted
			// The NCC must use snapshots on proxies for lag compensated raycasts to work properly against them.
			// The benefit of "Auto" is that it will update automatically if InputAuthority is changed (this is not relevant in this game, but worth keeping in mind)
			GetComponent<NetworkCharacterControllerPrototype>().InterpolationDataSource = InterpolationDataSources.Auto;
		}

		private void InitializeHealth()
		{
			
			if (playerID == Runner.LocalPlayer.PlayerId)
			{
				var _playerUI = PlayerNetworkManager.Instance.PlayerUI;
				_playerUI.SetActive(true);
				MyPlayerHealthModel = _playerUI.GetComponentInChildren<IHealthModel>();
				MyPlayerHealthModel.Parent = this;
			}
			else
			{
				var _enemyUI = PlayerNetworkManager.Instance.EnemyUI;
				_enemyUI.SetActive(true);
				MyPlayerHealthModel = _enemyUI.GetComponentInChildren<IHealthModel>();
				IsEnemy = true;
				MyPlayerHealthModel.Parent = this;
			}

			if (FusionLauncher.GameMode == GameMode.Single)
			{
				var _enemyUI = PlayerNetworkManager.Instance.EnemyUI;
				_enemyUI.SetActive(true);
			}

			Health = MAX_HEALTH;
			/*Health = MyPlayerHealth.GetHealth();

			MyPlayerHealth?.OnInitialize.Invoke(MAX_HEALTH);*/
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

		
		public static void OnStateChanged(Changed<PlayerNetworkObject> changed)
		{
			if(changed.Behaviour)
				changed.Behaviour.OnStateChanged();
		}
		
		public void OnStateChanged()
		{
			switch (state)
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
				case State.Despawned:
					//_teleportOut.StartTeleport();
					break;
			}
		}
		
		public static void OnHealthChanged(Changed<PlayerNetworkObject> changed)
		{
			if(changed.Behaviour)
				changed.Behaviour.OnHealthChanged();
		}

		public void OnHealthChanged()
		{
			MyPlayerHealthModel?.OnChangeHP.Invoke(Health);
		}

		public void OnComboChanged()
		{
			Debug.LogError("Behaviour.OnComboChanged");
		}

		private void ResetPlayer()
		{
			Debug.Log($"Resetting player {playerID}, tick={Runner.Simulation.Tick}, life={Health}, hasAuthority={Object.HasStateAuthority} to state={state}");
			//shooter.ResetAllWeapons();
			state = State.Active;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			Destroy(_deathExplosionInstance);
			PlayerNetworkManager.RemovePlayer(this);
		}
    }
}