using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListToyoIDData", menuName = "ScriptableObject/ListToyoIDData")]
    public class ListToyoIDData : ScriptableObject
    {
        public List<ToyoIDData> ListToyoData;
    }
