using Card;
using Card.CardStateMachine;
using Card.CardStateMachine.States;
using Card.CardUX;
using CardSystem.PlayerHand;
using TMPro;
using Tools;
using Tools.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class CardComponent : MonoBehaviour, ICard
{
    private void Awake()
    {
        MyTransform = transform;
        MyCollider = GetComponent<Collider>();
        MyRigidbody = GetComponent<Rigidbody>();
        MyImage = GetComponentInChildren<SpriteRenderer>();
        MyImages = GetComponentsInChildren<SpriteRenderer>();
        MyInput = GetComponent<IMouseInput>();

        Scale = new CardMotionScale(this);
        Movement = new CardMotionMovement(this);
        Rotation = new CardMotionRotation(this);
        
    }
    
    [Inject]
    public void Construct(IPlayerHand playerHand, SignalBus signalBus)
    {
        Hand = playerHand;
        _signalBus = signalBus;
    }

    private void Update()
    {
        if (StateMachine == null)
        {
            StateMachine = new CardStateMachine(MainCamera, _cardData, _signalBus, this);
            Disable();
        }
        else
        {
            StateMachine.Update();
            cardStateMachineStringDebug = StateMachine?.Current.ToString();
        }
        
        Movement?.Update();
        Rotation?.Update();
        Scale?.Update();
        
    }

    #region Properties

    [FormerlySerializedAs("MyCardData")] public CardData _cardData;

    private CardStateMachine StateMachine { get; set; }
    public string Name => _cardData.CardName ?? "NoName";
    public int CardID
    {
        get => _cardData.Unique_Card_ID;
        set => _ = value;
    }

    public bool IsDragging => StateMachine.IsCurrent<CardDrag>();
    public bool IsHovering => StateMachine.IsCurrent<CardHover>();
    public bool IsDisabled => StateMachine.IsCurrent<CardDisable>();

    [SerializeField] private string cardStateMachineStringDebug;

    [Header("Card Settings")] [SerializeField] private TextMeshPro MyDamageValue;
    [SerializeField] private TextMeshPro MyApCost;
    
    public MonoBehaviour MonoBehavior => this;
    public CardData CardData
    {
        get => _cardData;
        set
        {
            _cardData = value;
            DamageValue.text = _cardData.HitListInfos.Count > 0 
                ? _cardData.HitListInfos[0].Damage.ToString() : "0"; //Todo consider all damages
            APCost.text = _cardData.ApCost.ToString();
        }
    }

    public TextMeshPro DamageValue => MyDamageValue;
    public TextMeshPro APCost => MyApCost;


    public Camera MainCamera => Camera.main;

    private Transform MyTransform { get; set; }
    private SpriteRenderer[] MyImages { get; set; }
    private SpriteRenderer MyImage { get; set; }
    private Collider MyCollider { get; set; }
    private Rigidbody MyRigidbody { get; set; }
    private IMouseInput MyInput { get; set; }
    private IPlayerHand Hand { get; set; }
    private SignalBus _signalBus;

    public bool IsPlayer => transform.CloserEdge(MainCamera, Screen.width, Screen.height) == 1;

    #endregion


    #region Functions

    public void Disable()
    {
        StateMachine.Disable();
    }
    
    public void BlockUsage()
    {
        StateMachine.BlockUsage();
    }

    public void Enable()
    {
        StateMachine.Enable();
    }

    public void Select()
    {
        // to avoid the player selecting enemy's cards
        if (!IsPlayer)
            return;

        Hand.SelectCard(this);

        StateMachine.Select();
    }

    public void Unselect()
    {
        StateMachine.Unselect();
    }

    public void Hover()
    {
        StateMachine.Hover();
    }

    public void Draw()
    {
        StateMachine.Draw();
    }

    public void Conflict()
    {
        StateMachine.Conflict();
    }

    public void Queue()
    {
        StateMachine.Queue();
    }

    public void Destroy()
    {
        StateMachine.PlayDestroyAnimation();
    }

    public void Discard()
    {
        StateMachine.Discard();
    }

    #endregion

    #region Transform

    public void RotateTo(Vector3 rotation, float speed)
    {
        Rotation.Execute(rotation, speed);
    }

    public void MoveTo(Vector3 position, float speed, float delay)
    {
        Movement.Execute(position, speed, delay);
    }

    public void MoveToWithZ(Vector3 position, float speed, float delay)
    {
        Movement.Execute(position, speed, delay, true);
    }

    public void ScaleTo(Vector3 scale, float speed, float delay)
    {
        Scale.Execute(scale, speed, delay);
    }


    public CardMotionBase Movement { get; private set; }
    public CardMotionBase Rotation { get; private set; }
    public CardMotionBase Scale { get; private set; }

    #endregion


    #region Components

    SpriteRenderer[] ICardComponents.Images => MyImages;
    SpriteRenderer ICardComponents.Image => MyImage;
    Collider ICardComponents.Collider => MyCollider;
    Rigidbody ICardComponents.Rigidbody => MyRigidbody;
    IMouseInput ICardComponents.Input => MyInput;

    #endregion
}