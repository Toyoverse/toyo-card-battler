using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FakeDatabase", menuName = "ScriptableObject/FakeDatabase")]
public class FakeDatabase : ScriptableObject
{
    public List<PlayerTemporaryDatabase> PlayerRegistered;
}

[Serializable]
public class PlayerTemporaryDatabase
{
    public FullToyoSO FullToyoData;
    public int PlayerDatabaseID;
}