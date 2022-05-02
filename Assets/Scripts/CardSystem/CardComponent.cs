using Card;
using Card.CardStateMachine;
using Card.CardStateMachine.States;
using Card.CardUX;
using Extensions;
using PlayerHand;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class CardComponent : MonoBehaviour, ICard
{
    private void Awake()
    {
        MyTransform = transform;
        MyCollider = GetComponent<Collider>();
        MyRigidbody = GetComponent<Rigidbody>();
        MyImage = GetComponent<SpriteRenderer>();
        MyImages = GetComponentsInChildren<SpriteRenderer>();
        MyInput = GetComponent<IMouseInput>();
        Hand = GlobalConfig.Instance.battleReferences.hand.GetComponent<IPlayerHand>();

        Scale = new CardMotionScale(this);
        Movement = new CardMotionMovement(this);
        Rotation = new CardMotionRotation(this);

        StateMachine = new CardStateMachine(MainCamera, MyCardData, this);
    }

    private void Update()
    {
        StateMachine?.Update();
        Movement?.Update();
        Rotation?.Update();
        Scale?.Update();
    }

    #region Properties

    public CardData MyCardData;

    private CardStateMachine StateMachine { get; set; }
    public string Name => MyCardData.LocalizedName ?? "NoName";
    public bool IsDragging => StateMachine.IsCurrent<CardDrag>();
    public bool IsHovering => StateMachine.IsCurrent<CardHover>();
    public bool IsDisabled => StateMachine.IsCurrent<CardDisable>();

    [Header("Card Settings")] [SerializeField] private TextMeshPro MyDamageValue;
    [SerializeField] private TextMeshPro MyApCost;


    public MonoBehaviour MonoBehavior => this;
    public CardData CardData
    {
        get => MyCardData;
        set
        {
            MyCardData = value;
            DamageValue.text = MyCardData.HitListInfos[0].Damage.ToString(); //Todo consider all damages
            APCost.text = MyCardData.ApCost.ToString();
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

    public bool IsPlayer => transform.CloserEdge(MainCamera, Screen.width, Screen.height) == 1;

    #endregion


    #region Functions

    public void Disable()
    {
        StateMachine.Disable();
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