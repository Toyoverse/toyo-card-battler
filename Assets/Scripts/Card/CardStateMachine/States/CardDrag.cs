﻿using Extensions;
using Patterns.StateMachine;
using UnityEngine;

namespace Card.CardStateMachine.States
{
    public class CardDrag : CardBaseState
    {
        public CardDrag(Camera camera, ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler,
            stateMachine, cardData)
        {
            MyCamera = camera;
        }

        private Vector3 StartEuler { get; set; }
        private Camera MyCamera { get; }

        private Vector3 WorldPoint()
        {
            var _mousePosition = Handler.Input.MousePosition;
            var _worldPoint = MyCamera.ScreenToWorldPoint(_mousePosition);
            return _worldPoint;
        }

        private void FollowCursor()
        {
            var _myZ = Handler.transform.position.z;
            Handler.transform.position = WorldPoint().WithZ(_myZ);
        }

        #region Functions

        public override void OnUpdate()
        {
            FollowCursor();
        }

        public override void OnEnterState()
        {
            Handler.Movement.StopMotion();

            StartEuler = Handler.transform.eulerAngles;

            Handler.RotateTo(Vector3.zero, RotationSpeed);
            MakeRenderFirst();
            RemoveAllTransparency();
        }

        public override void OnExitState()
        {
            if (Handler.transform)
            {
                Handler.RotateTo(StartEuler, RotationSpeed);
                MakeRenderNormal();
            }

            DisableCollision();
        }

        #endregion
    }
}