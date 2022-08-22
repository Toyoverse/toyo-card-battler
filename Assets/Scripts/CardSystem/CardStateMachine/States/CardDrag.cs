using DefaultNamespace;
using Extensions;
using Patterns.StateMachine;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Card.CardStateMachine.States
{
    public class CardDrag : CardBaseState
    {
        public CardDrag(Camera camera, ICard handler, BaseStateMachine stateMachine,SignalBus signalBus, CardData cardData) : base(handler,
            stateMachine, signalBus, cardData)
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
            
            DisableCollision();

            StartEuler = Handler.transform.eulerAngles;

            Handler.RotateTo(Vector3.zero, GlobalCardData.RotationSpeed);
            MakeRenderFirst();
            RemoveAllTransparency();
        }

        public override void OnExitState()
        {
            EnableCollision();
            
            if (Handler.transform)
            {
                Handler.RotateTo(StartEuler, GlobalCardData.RotationSpeed);
                MakeRenderNormal();
            }

            DisableCollision();
        }

        #endregion
    }
}