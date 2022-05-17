using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tools
{

[CustomPropertyDrawer(typeof(EnableIf), true)]
[CustomPropertyDrawer(typeof(DisableIf), false)]
[CustomPropertyDrawer(typeof(ReadOnly))]
[CustomPropertyDrawer(typeof(DisplayAsString))]
[CustomPropertyDrawer(typeof(HideLabel))]
[CustomPropertyDrawer(typeof(MaxValue))]
[CustomPropertyDrawer(typeof(MinValue))]
[CustomPropertyDrawer(typeof(HideInInspector))]
[CustomPropertyDrawer(typeof(GUIColor))]
[CustomPropertyDrawer(typeof(IgnoreAttributes))]
[CustomPropertyDrawer(typeof(LabelText))]
[CustomPropertyDrawer(typeof(MinMax))]
[CustomPropertyDrawer(typeof(ProgressBar))]
[CustomPropertyDrawer(typeof(Doc))]
[CustomPropertyDrawer(typeof(InfoBox), true)]
[CustomPropertyDrawer(typeof(HorizontalLine), true)]
[CustomPropertyDrawer(typeof(Search))]
[CustomPropertyDrawer(typeof(IsDebug))]
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
        if (_customAttributes.OfType<IgnoreAttributes>().Any())
        {
            base.OnGUI(position, property, label);
            return;
        }

        if (_customAttributes.OfType<Search>().Any()&& fieldInfo.FieldType.ToString().Contains("[") == false && CustoBaseClass.search != "" && label.text.ToUpper().Contains(CustoBaseClass.search.ToUpper()) == false)
                color.a = 0.1f;

        GUI.color = color;
        
        if (_customAttributes.OfType<HideInInspector>().Any())
            return;
        
        if (_customAttributes.OfType<ScriptableObjectId>().Any())
            GUI.enabled = false;

        if (fieldInfo.GetType().IsArray == false)
        {
            if (_customAttributes.OfType<EnableIf>().Any() && !ValidadeEnableIf(property))
                GUI.enabled = false;

            if (_customAttributes.OfType<DisableIf>().Any() && ValidadeDisableIf(property))
                GUI.enabled = false;

            if (_customAttributes.OfType<ShowIf>().Any() && !ValidadeShowIf(property))
                return;

            if (_customAttributes.OfType<HideIf>().Any() && ValidadeHideIf(property))
                return;
            
        }
        if (_customAttributes.OfType<CustomType>().Any())
        {
            ProcessCustomType(property, position, label);
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        if (_customAttributes.OfType<ReadOnly>().Any())
            GUI.enabled = false;
        
        if(_customAttributes.OfType<DisplayAsString>().Any())
        {
            ProcessDisplayAsString(property, position);
            return;
        }
        
        if (_customAttributes.OfType<IsDebug>().Any() && attribute is IsDebug _isDebug)
        {
            ProcessIsDebug(_isDebug, position);
            return;
        }
        
        if (GUI.enabled && _customAttributes.OfType<InfoBox>().Any() && attribute is InfoBox _infoBox)
            ProcessInfoBox(_infoBox,position);

        if (_customAttributes.OfType<HorizontalLine>().Any())
            ProcessHorizontalLine(position);
        
        if (_customAttributes.OfType<GUIColor>().Any())
            ProcessGuiColor();

        if (_customAttributes.OfType<MaxValue>().Any())
            ProcessMaxValue(property);

        if (_customAttributes.OfType<MinValue>().Any())
            ProcessMinValue(property);

        if (_customAttributes.OfType<Doc>().Any())
            ProcessDoc(position);

        if (_customAttributes.OfType<LabelText>().Any())
            ProcessLabelText(label);
        
        if (_customAttributes.OfType<HideLabel>().Any())
        {
            EditorGUI.PropertyField(position, property, GUIContent.none);
            return;
        }
        if (_customAttributes.OfType<MinMax>().Any())
            ProcessMinMax(property, position, label);
        
        else if (_customAttributes.OfType<ProgressBar>().Any())
            ProcessProgressBar(property, position);
        
        else
            EditorGUI.PropertyField(position, property, label, true);

        
        if (_customAttributes.OfType<ScriptableObjectId>().Any())
            ProcessScriptableObjectId(property, position, label);
        
        EditorGUI.EndProperty();
        GUI.color = Color.white;
        GUI.enabled = true;
    }

    void ProcessCustomType(SerializedProperty property, Rect position, GUIContent label)
    {
        var selected = 0;
        CustomType ct = fieldInfo.GetCustomAttribute<CustomType>();
        EditorGUI.BeginProperty(position, label, property);

        if (ct.tp != "Tag" && ct.tp != "Layer" && !System.IO.File.Exists(Application.streamingAssetsPath + "/CustomTypes/" + ct.tp))
        {
            EditorGUI.LabelField(position, "System Type " + ct.tp + " not found");
            return;
        }

        string[] options = ct.tp == "Tag" ? UnityEditorInternal.InternalEditorUtility.tags : (ct.tp == "Layer") ? UnityEditorInternal.InternalEditorUtility.layers : System.IO.File.ReadAllLines(Application.streamingAssetsPath + "/CustomTypes/" + ct.tp);
        if (ct.typeField == CustomTypeField.STRING)
        {
            selected = GetSel(property.stringValue, options);
            selected = EditorGUI.Popup(position, label.text, selected, options);
            property.stringValue = options[selected];
        }
        else
        {
            selected = GetSelArray(property.stringValue, options);
            selected = EditorGUILayout.MaskField(label.text, selected, options);
            property.ClearArray();
            List<string> selectedOptions = new List<string>();
            for (int i = 0; i < options.Length; i++)
            {
                if ((selected & (1 << i)) == (1 << i)) selectedOptions.Add(options[i]);
            }
            property.arraySize = selectedOptions.Count;
            property.stringValue = "";
            for (int i = 0; i <= selectedOptions.Count - 1; i++)
            {
                property.stringValue += i == 0 ? selectedOptions[i] : ";" + selectedOptions[i];

            }
        }
        EditorGUI.EndProperty();
    }

    void ProcessDisplayAsString(SerializedProperty property, Rect position)
    {
        GUIStyle style = GUI.skin.box;
        style.fontStyle = FontStyle.Bold;         
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), property.stringValue,style);
    }

    void ProcessIsDebug(IsDebug _isDebug, Rect position)
    {
        if (_isDebug.Parameter != "PlayerManager.IsDebug") return;
        var cond = PlayerPrefs.GetInt("PlayerManagerIsDebug") == 1;
        GUIStyle style = GUI.skin.box;
        style.fontStyle = FontStyle.Bold;
        GUI.color = cond ? Color.red : Color.green;
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), cond ? _isDebug.IsTrue : _isDebug.IsFalse, style);
        GUI.color = Color.white;
    }

    void ProcessInfoBox(InfoBox _infoBox, Rect position)
    {
        EditorGUI.HelpBox(new Rect( position.x,position.y,position.width,27), _infoBox.info, MessageType.Info);
        position.y += 30;
    }

    void ProcessHorizontalLine(Rect position)
    {
        EditorGUI.DrawRect(new Rect(position.x, position.y, position.width, 1f), Color.grey);
        position.y += 5;
    }

    void ProcessMaxValue(SerializedProperty property)
    {
        var max = fieldInfo.GetCustomAttribute<MaxValue>();
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer when property.intValue > max.value:
                property.intValue = max.value;
                break;
            case SerializedPropertyType.Float when property.floatValue > max.valueFloat:
                property.floatValue = max.valueFloat;
                break;
        }
    }

    void ProcessGuiColor()
    {
        var gc = fieldInfo.GetCustomAttribute<GUIColor>();
        GUI.color = gc.color;
    }
    
    void ProcessMinValue(SerializedProperty property)
    {
        var min = fieldInfo.GetCustomAttribute<MinValue>();
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer when property.intValue < min.value:
                property.intValue = min.value;
                break;
            case SerializedPropertyType.Float when property.floatValue < min.valueFloat:
                property.floatValue = min.valueFloat;
                break;
        }
    }

    void ProcessDoc(Rect position)
    {
        if (GUI.Button(new Rect(position.x, position.y, 30f, position.height), "?"))
            Application.OpenURL(fieldInfo.GetCustomAttribute<Doc>().url);
            
        position.width -= 30f;
        position.x += 30;
    }

    void ProcessLabelText(GUIContent label)
    {
        var labelText = attribute as LabelText;
        label.text = labelText?.value;
    }
    
    void ProcessMinMax(SerializedProperty property, Rect position, GUIContent label)
    {
        var minmax = fieldInfo.GetCustomAttribute<MinMax>();
        var minValue = property.vector2Value.x;
        var maxValue = property.vector2Value.y;
        EditorGUI.MinMaxSlider(new Rect(position.x, position.y, position.width - 80, position.height), label, ref minValue, ref maxValue, minmax.min, minmax.max);
        EditorGUI.LabelField(new Rect(position.width - 60, position.y, 100, position.height), minValue.ToString("N2") + " | " + maxValue.ToString("N2"));
        var vec = Vector2.zero;
        vec.x = minValue;
        vec.y = maxValue;

        property.vector2Value = vec;
    }
    
    void ProcessProgressBar(SerializedProperty property, Rect position)
    {
        var pb = fieldInfo.GetCustomAttribute<ProgressBar>();
        if (armor == -1)
            armor = property.floatValue;
        GUI.Box(position, "");
        armor = EditorGUI.IntSlider(new Rect(position.x, position.y, position.width - 6, 15), pb.title, Mathf.RoundToInt(property.floatValue), pb.min, pb.max);
        EditorGUI.ProgressBar(new Rect(position.x, position.y + 20, position.width - 6, 20), ((armor - pb.min) / ((pb.max - pb.min))), pb.title);
        property.floatValue = armor;
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

    private int GetSel(string stringValue, string[] options)
    {
        for (var i = 0; i <= options.Length - 1; i++)
        {
            if (options[i] == stringValue)
                return i;
        }
        return 0;
    }
    
    private int GetSelArray(string stringValue, string[] options)
    {
        var ret = 0;

        var str = stringValue.Split(';');
        for (var i = 0; i <= str.Length - 1; i++)
        {
            var pot = 1;
            for (var ii = 0; ii <= options.Length - 1; ii++)
            {
                if (str[i] == options[ii])
                    if (ii == 0)
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret += pot;

                    }
                pot *= 2;
            }

        }

        return ret;
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var nt = base.GetPropertyHeight(property, label);
        if (fieldInfo.GetCustomAttribute<ProgressBar>() != null)
        {
            nt += 30f;
            return nt;
        }
        if (property.hasVisibleChildren)
            nt += (25 * property.CountInProperty());

        if (fieldInfo.GetCustomAttribute<HorizontalLine>() != null)
            nt += 6f;

        if (GUI.enabled && attribute is InfoBox infoBox)
            nt += 30f;
        
        return nt;

    }

    bool ValidadeEnableIf(SerializedProperty property)
    {
        return attribute is not EnableIf _serializedProperty || ValidatePropertyCondition(property, _serializedProperty);
    }
    bool ValidadeDisableIf(SerializedProperty property)
    {
        return attribute is not DisableIf _serializedProperty || ValidatePropertyCondition(property, _serializedProperty);
    }

    bool ValidadeShowIf(SerializedProperty property)
    {
        return attribute is not ShowIf _serializedProperty || ValidatePropertyCondition(property, _serializedProperty);
    }
    
    bool ValidadeHideIf(SerializedProperty property)
    {
        return attribute is not HideIf _serializedProperty || ValidatePropertyCondition(property, _serializedProperty);
    }
    

    bool ValidatePropertyCondition(SerializedProperty property, PropertyGroupAttribute _serializedProperty)
    {
        var _containsAND = _serializedProperty.parameter.Contains("&&");
        
        if (_serializedProperty.parameter.Contains("@") || _serializedProperty.parameter.Contains("||") || _containsAND)
        {
            var _conditionArray = PrepareStringForValidation(_serializedProperty.parameter);
            var pass = ValidadeCondition(property, _conditionArray);
            if (!(pass == _conditionArray.Length || !_containsAND && pass > 0))
                return false;
        }
        else
        {
            var objp = PropertyUtility.GetTargetObjectWithProperty(property);
            var field = ReflectionUtility.GetField(objp, _serializedProperty.parameter);
            var obj = field.GetValue(objp);
            if (obj?.ToString() != _serializedProperty.condition.ToString())
                return false;
        }

        return true;
    }
    
    string[] PrepareStringForValidation(string _string)
    {
        var _formattedString = _string.Replace("||", "|").Replace("@", "").Replace(" ", "").Replace("&&", "|");
        return _formattedString.Split('|');
    }

    int ValidadeCondition(SerializedProperty property, string[] _conditionArray)
    { 
        return (from _eachCondition in _conditionArray
                select _eachCondition.Replace("==", "=").Split('=')
                into s2
                where s2.Length >= 2
                let objp = PropertyUtility.GetTargetObjectWithProperty(property)
                let field = ReflectionUtility.GetField(objp, s2[0])
                let obj = field.GetValue(objp)
                where obj != null
                let res = obj.ToString()
                where string.Equals(res, s2[1], StringComparison.CurrentCultureIgnoreCase)
                select s2
                ).Count();
    }
    
    private object GetValues(SerializedProperty property, string valuesName)
    {
        object target = PropertyUtility.GetTargetObjectWithProperty(property);

        FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, valuesName);
        if (valuesFieldInfo != null)
        {
            return valuesFieldInfo.GetValue(target);
        }

        PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, valuesName);
        if (valuesPropertyInfo != null)
        {
            return valuesPropertyInfo.GetValue(target);
        }

        MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, valuesName);
        if (methodValuesInfo != null &&
            methodValuesInfo.ReturnType != typeof(void) &&
            methodValuesInfo.GetParameters().Length == 0)
        {
            return methodValuesInfo.Invoke(target, null);
        }

        return null;
    }
}
}

