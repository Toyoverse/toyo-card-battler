using Card.CardUX;
using UnityEngine;

namespace Card
{
    public interface ICardTransform
    {
        CardMotionBase Movement { get; }
        CardMotionBase Rotation { get; }
        CardMotionBase Scale { get; }

        void MoveTo(Vector3 position, float speed, float delay = 0);

        void MoveToWithZ(Vector3 position, float speed, float delay = 0);

        void RotateTo(Vector3 euler, float speed);

        void ScaleTo(Vector3 scale, float speed, float delay = 0);
    }
}