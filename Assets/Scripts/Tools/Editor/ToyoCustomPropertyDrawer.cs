using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor
{

    [CustomPropertyDrawer(typeof(ScriptableObjectId))]
    public class ToyoCustomPropertyDrawer : PropertyDrawer
    {
        float armor = -1;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            GUI.enabled = true;
            var color = GUI.color;
            color.a = 1f;

            var _customAttributes = fieldInfo.GetCustomAttributes().ToArray();

            if (_customAttributes.OfType<ScriptableObjectId>().Any())
            {
                GUI.enabled = false;
                ProcessScriptableObjectId(property, position, label);
            }
                
            
            EditorGUI.EndProperty();
            GUI.color = Color.white;
            GUI.enabled = true;
        }
      
        void ProcessScriptableObjectId(SerializedProperty property, Rect position, GUIContent label)
        {
            if (property.intValue == 0)
            {
                var newValue = Resources.Load<IndexerSO>("IndexerSO").CardIndex ++;
                property.intValue = (int) newValue;
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

       
    }
}

