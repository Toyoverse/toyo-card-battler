using DefaultNamespace;
using Extensions;
using Patterns.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card.CardStateMachine.States
{
    public class CardHover : CardBaseState
    {
        public CardHover(ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine,
            cardData)
        {
        }

        private void OnPointerExit(PointerEventData obj)
        {
            if (StateMachine.IsCurrent(this))
                Handler.Enable();
        }

        private void OnPointerDown(PointerEventData eventData)
        {
            if (StateMachine.IsCurrent(this))
                Handler.Select();
        }


        private void ResetValues()
        {
            var rotationSpeed = Handler.IsPlayer ? GlobalCardData.RotationSpeed : GlobalCardData.RotationSpeedEnemy;
            Handler.RotateTo(StartEuler, rotationSpeed);
            Handler.MoveTo(StartPosition, GlobalCardData.HoverSpeed);
            Handler.ScaleTo(StartScale, GlobalCardData.ScaleSpeed);
        }

        private void SetRotation()
        {
            if (GlobalCardData.HoverRotation)
                return;

            var speed = Handler.IsPlayer ? GlobalCardData.RotationSpeed : GlobalCardData.RotationSpeedEnemy;

            Handler.RotateTo(Vector3.zero, speed);
        }

        private void SetPosition()
        {
            var camera = Handler.MainCamera;
            var halfCardHeight = new Vector3(0, Handler.Image.bounds.size.y / 2);
            var bottomEdge = Handler.MainCamera.ScreenToWorldPoint(Vector3.zero);
            var topEdge = Handler.MainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height));
            var edgeFactor = Handler.transform.CloserEdge(camera, Screen.width, Screen.height);
            var myEdge = edgeFactor == 1 ? bottomEdge : topEdge;
            var edgeY = new Vector3(0, myEdge.y);
            var currentPosWithoutY = new Vector3(Handler.transform.position.x, 0, Handler.transform.position.z);
            var hoverHeightParameter = new Vector3(0, GlobalCardData.HoverHeight);
            var final = currentPosWithoutY + edgeY + (halfCardHeight + hoverHeightParameter) * edgeFactor;
            Handler.MoveTo(final, GlobalCardData.HoverSpeed);
        }

        private void SetScale()
        {
            var currentScale = Handler.transform.localScale;
            var finalScale = currentScale * GlobalCardData.HoverScale;
            Handler.ScaleTo(finalScale, GlobalCardData.ScaleSpeed);
        }

        private void CachePreviousValues()
        {
            StartPosition = Handler.transform.position;
            StartEuler = Handler.transform.eulerAngles;
            StartScale = Handler.transform.localScale;
        }

        private void SubscribeInput()
        {
            Handler.Input.OnPointerExit += OnPointerExit;
            Handler.Input.OnPointerDown += OnPointerDown;
        }

        private void UnsubscribeInput()
        {
            Handler.Input.OnPointerExit -= OnPointerExit;
            Handler.Input.OnPointerDown -= OnPointerDown;
        }

        private void CalcEdge()
        {
        }


        #region Operations

        public override void OnEnterState()
        {
            MakeRenderFirst();
            SubscribeInput();
            CachePreviousValues();
            SetScale();
            SetPosition();
            SetRotation();
        }

        public override void OnExitState()
        {
            ResetValues();
            UnsubscribeInput();
            DisableCollision();
        }

        #endregion


        #region Properties

        private Vector3 StartPosition { get; set; }
        private Vector3 StartEuler { get; set; }
        private Vector3 StartScale { get; set; }

        #endregion
    }
}