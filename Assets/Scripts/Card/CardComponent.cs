using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Card;
using Card.CardStateMachine;
using Card.CardStateMachine.States;
using Card.CardUX;
using PlayerHand;
using Tools.UI;
using UnityEngine;

public class CardComponent : MonoBehaviour, ICard
{
    private void Awake()
    {
        MyTransform = transform;
        MyCollider = GetComponent<Collider>();
        MyRigidbody = GetComponent<Rigidbody>();
        MyRenderer = GetComponent<SpriteRenderer>();
        MyRenderers = GetComponentsInChildren<SpriteRenderer>();
        MyInput = GetComponent<IMouseInput>();
        
        Scale = new CardMotionScale(this);
        Movement = new CardMotionMovement(this);
        Rotation = new CardMotionRotation(this);
        
        StateMachine = new CardStateMachine(MainCamera, cardData, this);
        
    }

    void Update()
    {
        StateMachine?.Update();
        Movement?.Update();
        Rotation?.Update();
        Scale?.Update();
    }

    #region Properties

    [Header("Card Settings")]
    [SerializeField] public CardData cardData;
    
    CardStateMachine StateMachine { get; set; }
    public string Name => cardData.localizedName ?? "NoName";
    public bool IsDragging => StateMachine.IsCurrent<CardDrag>();
    public bool IsHovering => StateMachine.IsCurrent<CardHover>();
    public bool IsDisabled => StateMachine.IsCurrent<CardDisable>();
    
    
    public MonoBehaviour MonoBehavior => this;
    public Camera MainCamera => Camera.main;
    
    Transform MyTransform { get; set; }
    SpriteRenderer[] MyRenderers { get; set; }
    SpriteRenderer MyRenderer { get;  set;}
    Collider MyCollider { get;  set;}
    Rigidbody MyRigidbody { get;  set;}
    IMouseInput MyInput { get; set; }
    private IPlayerHand Hand { get; set; }

    public bool IsPlayer => transform.CloserEdge(MainCamera, Screen.width, Screen.height) == 1;
    
    #endregion


    #region Functions

    public void Disable() => StateMachine.Disable();

    public void Enable() => StateMachine.Enable(); 

    public void Select()
    {
        // to avoid the player selecting enemy's cards
        if (!IsPlayer)
            return;

        Hand.SelectCard(this);
        
        StateMachine.Select();
    }

    public void Unselect() => StateMachine.Unselect();

    public void Hover() => StateMachine.Hover();

    public void Draw() => StateMachine.Draw();

    public void Discard() => StateMachine.Discard();

    #endregion
    
    #region Transform

    public void RotateTo(Vector3 rotation, float speed) => Rotation.Execute(rotation, speed);

    public void MoveTo(Vector3 position, float speed, float delay) => Movement.Execute(position, speed, delay);

    public void MoveToWithZ(Vector3 position, float speed, float delay) => Movement.Execute(position, speed, delay, true);

    public void ScaleTo(Vector3 scale, float speed, float delay) => Scale.Execute(scale, speed, delay);
    

    public CardMotionBase Movement { get; private set; }
    public CardMotionBase Rotation { get; private set; }
    public CardMotionBase Scale { get; private set;}

    #endregion


    #region Components

     SpriteRenderer[] ICardComponents.Renderers => MyRenderers;
     SpriteRenderer ICardComponents.Renderer => MyRenderer;
     Collider ICardComponents.Collider => MyCollider;
     Rigidbody ICardComponents.Rigidbody => MyRigidbody;
     IMouseInput ICardComponents.Input => MyInput;

    #endregion


}
