using System;
using Tools;
using UnityEngine;

namespace HealthSystem
{
    public interface IHealthPresenter 
    {
        Action<float> OnUpdateHealthUI { get; set; }
        SpriteRenderer Renderer { get; }
        IMouseInput Input { get; }
        MonoBehaviour MonoBehaviour { get; }
        GameObject GameObject { get; }
        Transform Transform { get; }
        Camera MainCamera { get; }
    }
}