using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ListToyoIDData", menuName = "ScriptableObject/ListToyoIDData")]
    public class ListToyoIDData : ScriptableObject
    {
        public List<ToyoIDData> ListToyoData;
    }
