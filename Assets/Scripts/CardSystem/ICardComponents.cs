﻿using CardSystem.PlayerHand;
using TMPro;
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
        IPlayerHand PlayerHand { get; }
        MonoBehaviour MonoBehavior { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
        CardData CardData { get; set; }
        TextMeshPro DamageValue { get; }
        TextMeshPro APCost { get; }
        int CardID { get; set; }
    }
}