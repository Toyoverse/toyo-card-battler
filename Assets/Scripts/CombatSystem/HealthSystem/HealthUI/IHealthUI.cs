using System;
using Tools;
using UnityEngine;

namespace HealthSystem.HealthUI
{
    public interface IHealthUI 
    {
        Action<float> OnUpdateHealthUI { get; set; }
        SpriteRenderer Renderer { get; }
        IMouseInput Input { get; }
        MonoBehaviour MonoBehaviour { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        Camera MainCamera { get; }
    }
}