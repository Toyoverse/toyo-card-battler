using UnityEngine;

public class ScriptableObjectId : PropertyAttribute { }

public class UniqueScriptableObject : ScriptableObject {
    [ScriptableObjectId] 
    public int Id = 0;
}