using UnityEngine;
using UnityEngine.Serialization;

namespace Tools
{
    public class ScriptableObjectId : PropertyAttribute { }

    public class UniqueScriptableObject : ScriptableObject {
        [FormerlySerializedAs("Id")] [ScriptableObjectId] 
        public int Unique_Card_ID = 0;
    }
}