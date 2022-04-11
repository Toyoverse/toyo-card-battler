using System;
using Tools;
using UnityEngine;

namespace HealthSystem.HealthUI
{
    public interface IHealthUI 
    {
        Action<float, float> OnUpdateHealthUI { get; set; }
        SpriteRenderer Renderer { get; }
        IMouseInput Input { get; }
        IHealth Health { get; }
        MonoBehaviour MonoBehaviour { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        Camera MainCamera { get; }
    }
}