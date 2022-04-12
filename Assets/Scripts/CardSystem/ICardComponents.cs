using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Card
{
    public interface ICardComponents
    {
        Camera MainCamera { get; }
        SpriteRenderer[] Images { get; }
        SpriteRenderer Image { get; }
        Collider Collider { get; }
        Rigidbody Rigidbody { get; }
        IMouseInput Input { get; }
        MonoBehaviour MonoBehavior { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        CardData CardData { get; }
    }
}