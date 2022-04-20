using System;
using Tools;
using UnityEngine;

namespace APSystem.ApUI
{
    public interface IApUI 
    {
        Action<float> OnUpdateAPUI { get; set; }
        SpriteRenderer Renderer { get; }
        IMouseInput Input { get; }
        MonoBehaviour MonoBehaviour { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        Camera MainCamera { get; }
    }
}