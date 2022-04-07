using UnityEngine;

namespace Card.CardUX
{
    public class CardMotionMovement : CardMotionBase
    {
        public CardMotionMovement(ICard handler) : base(handler)
        {
        }

        private bool moveZ { get; set; }

        public override void Execute(Vector3 position, float speed, float delay, bool _moveZ)
        {
            //Store the global moveZ variable
            moveZ = _moveZ;
            base.Execute(position, speed, delay, _moveZ);
        }

        protected override void OnMotionEnds()
        {
            moveZ = false;
            IsOperating = false;
            var _target = Target;
            _target.z = Handler.transform.position.z;
            Handler.transform.position = _target;
            base.OnMotionEnds();
        }

        protected override bool CheckFinalState()
        {
            var _distance = Target - Handler.transform.position;
            if (moveZ)
                _distance.z = 0;
            return _distance.magnitude <= Threshold;
        }

        protected override void KeepMotion()
        {
            var _current = Handler.transform.position;
            var _amount = Speed * Time.deltaTime;
            var _delta = Vector3.Lerp(_current, Target, _amount);
            if (!moveZ)
                _delta.z = Handler.transform.position.z;
            Handler.transform.position = _delta;
        }
    }
}