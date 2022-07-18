using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "ToyoIDData", menuName = "ScriptableObject/ToyoIDData")]
    public class ToyoIDData : UniqueScriptableObject
    {
        public string Name;

        public TOYO_PIECE ToyoPiece;
    }
