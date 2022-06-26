using System;
using System.Collections.Generic;
using Card;
using Sirenix.OdinInspector;
using ToyoSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "FullToyo", menuName = "ScriptableObject/FullToyo")]
public class FullToyoSO : ScriptableObject
{
    public List<ToyoPartSO> ToyoParts;
}

