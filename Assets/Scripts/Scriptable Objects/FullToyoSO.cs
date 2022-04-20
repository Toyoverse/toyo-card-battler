using System;
using System.Collections.Generic;
using Card;
using Tools;
using ToyoSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "FullToyo", menuName = "ScriptableObject/FullToyo")]
[UseAttributes]
public class FullToyoSO : ScriptableObject
{
    public List<ToyoPartSO> ToyoParts;
}

