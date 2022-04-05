using Extensions;
using Patterns.StateMachine;
using UnityEngine;

namespace Card.CardStateMachine.States
{
    public class CardDrag : CardBaseState
    {
        public CardDrag(Camera camera, ICard handler, BaseStateMachine stateMachine, CardData cardData) : base(handler, stateMachine, cardData)
        {
            MyCamera = camera;
        }
        
        Vector3 StartEuler { get; set; }
        Camera MyCamera { get; }
        
        Vector3 WorldPoint()
        {
            var _mousePosition = Handler.Input.MousePosition;
            var _worldPoint = MyCamera.ScreenToWorldPoint(_mousePosition);
            return _worldPoint;
        }

        void FollowCursor()
        {
            var _myZ = Handler.transform.position.z;
            Handler.transform.position = WorldPoint().WithZ(_myZ);
        }

        #region Functions
        
        public override void OnUpdate() => FollowCursor();

        public override void OnEnterState()
        {
            //stop any movement
            Handler.Movement.StopMotion();

            //cache old values
            StartEuler = Handler.transform.eulerAngles;

            Handler.RotateTo(Vector3.zero, RotationSpeed);
            MakeRenderFirst();
            RemoveAllTransparency();
        }

        public override void OnExitState()
        {
            //reset position and rotation
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