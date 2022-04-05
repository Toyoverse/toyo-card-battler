using Tools.UI;
using UnityEngine;

namespace Card
{
    public interface ICardComponents
    {
        Camera MainCamera { get; }
        SpriteRenderer[] Renderers { get; }
        SpriteRenderer Renderer { get; }
        Collider Collider { get; }
        Rigidbody Rigidbody { get; }
        IMouseInput Input { get; }
        MonoBehaviour MonoBehavior { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}