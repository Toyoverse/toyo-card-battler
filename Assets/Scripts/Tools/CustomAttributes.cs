using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools
{
    public class CustomAttributes : MonoBehaviour
    {
    }

#if UNITY_EDITOR

    #region Custom Class

    
    [CustomEditor(typeof(ScriptableObject), true)]
    public class CustomEditorScriptObj : CustomToyoEditor
    {
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class CustoBaseClass : CustomToyoEditor
    {
    }

    [CustomEditor(typeof(Search), true)]
    [CanEditMultipleObjects]
    public class CustoBaseClassSearch : CustomToyoEditor
    {
    }

    #endregion

    #region Processor class

    public class CustomToyoEditor : Editor
    {
        public static string search = "";

        private List<FieldInfo> fields = new();
        private readonly List<bool> listBool = new();
        private readonly List<string> listProp = new();
        private List<PropertyInfo> properties = new();

        private void OnEnable()
        {
            fields = ReflectionUtility.GetAllFields(target);
            properties = ReflectionUtility.GetAllProps(target);
            if (target.GetType().GetCustomAttribute<IgnoreAttributes>() != null) return;
            if (target.GetType() == typeof(MonoBehaviour) &&
                target.GetType().GetCustomAttribute<UseAttributes>() == null) return;
            var property = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var prop in property)
            {
                var p = prop.GetCustomAttribute<FoldoutGroup>();
                if (p != null)
                    if (listProp.Contains(p.name) == false)
                    {
                        listProp.Add(p.name);
                        listBool.Add(false);
                    }
            }

            var propertys = target.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var prop in propertys)
            {
                var p = prop.GetCustomAttribute<FoldoutGroup>();
                if (p != null)
                    if (listProp.Contains(p.name) == false)
                    {
                        listProp.Add(p.name);
                        listBool.Add(false);
                    }
            }
        }

        public override void OnInspectorGUI()
        {
            if (target.GetType().GetCustomAttribute<IgnoreAttributes>() != null)
            {
                base.OnInspectorGUI();
                return;
            }

            if (target.GetType().BaseType == typeof(MonoBehaviour) &&
                target.GetType().GetCustomAttribute<UseAttributes>() == null)
            {
                base.OnInspectorGUI();
                return;
            }

            serializedObject.Update();

            var i = -1;
            var oldProp = "";
            foreach (var l in listProp)
            {
                GUI.enabled = true;
                var rect = EditorGUILayout.BeginVertical();
                GUI.Box(rect, GUIContent.none);
                if (l != oldProp)
                {
                    oldProp = l;
                    i++;
                }

                listBool[i] = EditorGUILayout.Foldout(listBool[i], oldProp); // || search != "";
                if (listBool[i])
                {
                    GUI.color = Color.white;

                    foreach (var prop in GetProps(oldProp))
                        if (ProcessProp(prop))
                            if (search != "")
                                listBool[i] = true;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }

            var listpr = GetProps("");
            if (listProp.Count > 0 && listpr.Count > 0)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("--- Uncategorized attributes ---");
            }

            foreach (var prop in listpr)
                ProcessProp(prop);
            serializedObject.ApplyModifiedProperties();
        }

        private bool ProcessProp(FieldMetInfo prop)
        {
            if (prop.field != null && prop.field.GetType().IsArray) GUI.enabled = true;
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.yellow;

            var Cstyle = new GUIStyle(GUI.skin.button);


            if (prop.method != null)
            {
                var sib = prop.method.GetCustomAttribute<Button>();
                if (sib != null)
                {
                    var n = sib.name == "this" ? prop.method.Name : sib.name;
                    if (GUILayout.Button(n)) prop.method.Invoke(target, null);
                }

                return true;
            }

            if (prop.field != null)
            {
                var sif = prop.field.GetCustomAttribute<ShowIf>();
                if (sif != null)
                {
                    if (sif.parameter.Contains("@") || sif.parameter.Contains("||") || sif.parameter.Contains("&&"))
                    {
                        var eE = sif.parameter.Contains("&&");
                        var str = sif.parameter.Replace("||", "|").Replace("@", "").Replace(" ", "").Replace("&&", "|");
                        var lstr = str.Split('|');
                        var pass = 0;
                        foreach (var ll in lstr)
                        {
                            var s2 = ll.Replace("==", "=").Split('=');
                            if (s2.Length < 2)
                            {
                            }
                            else
                            {
                                var obj = target.GetType().GetField(s2[0])?.GetValue(target);
                                if (obj != null)
                                {
                                    var res = obj.ToString();
                                    if (res.ToLower() == s2[1].ToLower()) pass++;
                                }
                            }
                        }

                        if (pass == lstr.Length || !eE && pass > 0)
                        {
                            //Not
                        }
                        else

                        {
                            return false;
                        }
                    }
                    else
                    {
                        var obj = target.GetType().GetField(sif.parameter)?.GetValue(target);
                        if (obj != null)
                            if (obj.ToString() != sif.condition.ToString())
                                return false;
                    }
                }

                var snif = prop.field.GetCustomAttribute<HideIf>();
                if (snif != null)
                {
                    if (snif.parameter.Contains("@") || snif.parameter.Contains("||") || snif.parameter.Contains("&&"))
                    {
                        var eE = snif.parameter.Contains("&&");
                        var str = snif.parameter.Replace("||", "|").Replace("@", "").Replace(" ", "")
                            .Replace("&&", "|");
                        var lstr = str.Split('|');
                        var pass = 0;
                        foreach (var ll in lstr)
                        {
                            var s2 = ll.Replace("==", "=").Split('=');
                            if (s2.Length < 2)
                            {
                            }
                            else
                            {
                                var obj = target.GetType().GetField(s2[0])?.GetValue(target);
                                if (obj != null)
                                {
                                    var res = obj.ToString();
                                    if (res.ToLower() == s2[1].ToLower()) pass++;
                                }
                            }
                        }

                        if (pass == lstr.Length || !eE && pass > 0)
                            return false;
                    }
                    else
                    {
                        var obj = target.GetType().GetField(snif.parameter)?.GetValue(target);
                        if (obj != null)
                            if (obj.ToString() == snif.condition.ToString())
                                return false;
                    }
                }
            }

            if (prop.prop != null)
            {
                var sif = prop.prop.GetCustomAttribute<ShowIf>();
                if (sif != null)
                {
                    if (sif.parameter.Contains("@") || sif.parameter.Contains("||") || sif.parameter.Contains("&&"))
                    {
                        var eE = sif.parameter.Contains("&&");
                        var str = sif.parameter.Replace("||", "|").Replace("@", "").Replace(" ", "").Replace("&&", "|");
                        var lstr = str.Split('|');
                        var pass = 0;
                        foreach (var ll in lstr)
                        {
                            var s2 = ll.Replace("==", "=").Split('=');
                            if (s2.Length < 2)
                            {
                            }
                            else
                            {
                                var obj = target.GetType().GetProperty(s2[0])?.GetValue(target);
                                if (obj != null)
                                {
                                    var res = obj.ToString();
                                    if (res.ToLower() == s2[1].ToLower()) pass++;
                                }
                            }
                        }

                        if (pass == lstr.Length || !eE && pass > 0)
                        {
                            //Not
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var obj = target.GetType().GetProperty(sif.parameter)?.GetValue(target);
                        if (obj != null)
                            if (obj.ToString() != sif.condition.ToString())
                                return false;
                    }
                }

                var snif = prop.prop.GetCustomAttribute<HideIf>();
                if (snif != null)
                {
                    if (snif.parameter.Contains("@") || snif.parameter.Contains("||") || snif.parameter.Contains("&&"))
                    {
                        var eE = snif.parameter.Contains("&&");
                        var str = snif.parameter.Replace("||", "|").Replace("@", "").Replace(" ", "")
                            .Replace("&&", "|");
                        var lstr = str.Split('|');
                        var pass = 0;
                        foreach (var ll in lstr)
                        {
                            var s2 = ll.Replace("==", "=").Split('=');
                            if (s2.Length < 2)
                            {
                            }
                            else
                            {
                                var obj = target.GetType().GetProperty(s2[0])?.GetValue(target);
                                if (obj != null)
                                {
                                    var res = obj.ToString();
                                    if (res.ToLower() == s2[1].ToLower()) pass++;
                                }
                            }
                        }

                        if (pass == lstr.Length || !eE && pass > 0)
                            return false;
                    }
                    else
                    {
                        var obj = target.GetType().GetProperty(snif.parameter)?.GetValue(target);
                        if (obj != null)
                            if (obj.ToString() == snif.condition.ToString())
                            {
                                var gs = new GUIStyle();
                                gs.alignment = TextAnchor.MiddleLeft;
                                if (GUILayout.Button(prop.field.Name + " - (Hide)", gs))
                                    EditorUtility.DisplayDialog("Hide because", sif.parameter + " = " + sif.condition,
                                        "OK");
                                return false;
                            }
                    }
                }
            }

            try
            {
                if (prop.field != null && prop.field.IsStatic == false && prop.field.IsPrivate == false &&
                    prop.field.IsNotSerialized == false && serializedObject.FindProperty(prop.field.Name) != null)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.field.Name), true);
                if (prop.prop != null && serializedObject.FindProperty(prop.prop.Name) != null)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.prop.Name), true);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message + ": " + prop.field?.Name);
                return false;
            }
        }

        private bool AttributeFound(FieldMetInfo prop)
        {
            if (prop.field != null && prop.field.Name.ToUpper().Contains(search.ToUpper()))
                return true;

            if (prop.method != null && prop.method.Name.ToUpper().Contains(search.ToUpper())) return true;
            if (prop.prop != null && prop.prop.Name.ToUpper().Contains(search.ToUpper())) return true;


            return false;
        }

        private List<FieldMetInfo> GetProps(string attr)
        {
            var ret = new List<FieldMetInfo>();
            var property =
                target.GetType()
                    .GetAllFields(); // target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default);
            foreach (var prop in property)
            {
                // Debug.Log(prop.FieldType + ":::" + prop.Name);
                if (prop.IsStatic) continue;
                var p = prop.GetCustomAttribute<FoldoutGroup>();
                var ph = prop.GetCustomAttribute<HorizontalGroup>();
                var pb = prop.GetCustomAttribute<BoxGroup>();


                if (p != null)
                {
                    if (p.name == attr || attr == "all") ret.Add(new FieldMetInfo { field = prop });
                }
                else if (ph != null)
                {
                    if (ph.name == attr || attr == "all") ret.Add(new FieldMetInfo { field = prop });
                }
                else if (pb != null)
                {
                    if (pb.name == attr || attr == "all") ret.Add(new FieldMetInfo { field = prop });
                }
                else if (attr == "" || attr == "all")
                {
                    ret.Add(new FieldMetInfo { field = prop });
                }
            }

            var propertys =
                target.GetType()
                    .GetAllProperties(); // target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default);
            foreach (var prop in propertys)
            {
                // if (prop.IsStatic) continue;
                var p = prop.GetCustomAttribute<FoldoutGroup>();
                var ph = prop.GetCustomAttribute<HorizontalGroup>();
                var pb = prop.GetCustomAttribute<BoxGroup>();
                if (p != null)
                {
                    if (p.name == attr || attr == "all") ret.Add(new FieldMetInfo { prop = prop });
                }
                else if (ph != null)
                {
                    if (ph.name == attr || attr == "all") ret.Add(new FieldMetInfo { prop = prop });
                }
                else if (pb != null)
                {
                    if (pb.name == attr || attr == "all") ret.Add(new FieldMetInfo { prop = prop });
                }
                else if (attr == "" || attr == "all")
                {
                    ret.Add(new FieldMetInfo { prop = prop });
                }
            }

            var mets = target.GetType().GetMethods();
            foreach (var prop in mets)
            {
                var p = prop.GetCustomAttribute<FoldoutGroup>();
                var ph = prop.GetCustomAttribute<HorizontalGroup>();
                var pb = prop.GetCustomAttribute<BoxGroup>();
                if (p != null)
                {
                    if (p.name == attr || attr == "all") ret.Add(new FieldMetInfo { method = prop });
                }
                else if (ph != null)
                {
                    if (ph.name == attr || attr == "all") ret.Add(new FieldMetInfo { method = prop });
                }
                else if (pb != null)
                {
                    if (pb.name == attr || attr == "all") ret.Add(new FieldMetInfo { method = prop });
                }
                else if (attr == "" || attr == "all")
                {
                    ret.Add(new FieldMetInfo { method = prop });
                }
            }

            return ret;
        }

        private List<FieldInfo> GetPropsInfo()
        {
            var property = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var ret = new List<FieldInfo>();
            foreach (var prop in property)
            {
                var p = prop.GetCustomAttribute<InfoBox>();
                if (p != null) ret.Add(prop);
            }

            return ret;
        }
    }

    #endregion

#endif

    #region Attribute Class

    public abstract class PropertyGroupAttribute : PropertyAttribute
    {
        public object condition;
        public string parameter = "";
    }

    #endregion

    #region Attributes

    /// <summary>
    ///     Show the value as a Label in read-only mode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelText : PropertyGroupAttribute
    {
        public string value;

        public LabelText(string label)
        {
            value = label;
        }
    }

    /// <summary>
    ///     Determines the minimum value for a numeric field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MinValue : PropertyGroupAttribute
    {
        public int value;
        public float valueFloat;

        public MinValue(int min)
        {
            value = min;
        }

        public MinValue(float min)
        {
            valueFloat = min;
        }
    }

    /// <summary>
    ///     Determines the maximun value for a numeric field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MaxValue : PropertyGroupAttribute
    {
        public int value;
        public float valueFloat;

        public MaxValue(int max)
        {
            value = max;
        }

        public MaxValue(float max)
        {
            valueFloat = max;
        }
    }

    /// <summary>
    ///     Mark the field read-only
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnly : PropertyGroupAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class HorizontalLine : PropertyGroupAttribute
    {
    }


    public enum CustomTypeField
    {
        STRING,
        ARRAY
    }

    /// <summary>
    ///     Cria um combobox de itens customozaveis, não pode ser usado em conjunto com outro atibuto
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class CustomType : PropertyGroupAttribute
    {
        public string tp;
        public CustomTypeField typeField;

        public CustomType(string _type, CustomTypeField customTypeField)
        {
            tp = _type;
            typeField = customTypeField;
        }
    }

    /// <summary>
    ///     Show value as read-only text
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAsString : PropertyGroupAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class IsDebug : PropertyGroupAttribute
    {
        public string Parameter, IsTrue, IsFalse;

        public IsDebug(string parameter, string isTrue, string isFalse)
        {
            Parameter = parameter;
            IsTrue = isTrue;
            IsFalse = isFalse;
        }
    }

    /// <summary>
    ///     Show value as read-only text
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HideLabel : PropertyGroupAttribute
    {
    }

    /// <summary>
    ///     Change the color of the field in the Inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GUIColor : PropertyGroupAttribute
    {
        public Color color;

        public GUIColor(float r, float g, float b)
        {
            color = new Color(r, g, b);
        }
    }

    /// <summary>
    ///     Draw a button in Inpector to invoke a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class Button : PropertyGroupAttribute
    {
        public string name = "";

        public Button()
        {
            name = "this";
        }

        public Button(string name)
        {
            this.name = name.Replace("/", "-");
        }
    }

    /// <summary>
    ///     Group the Items of the same flag
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class FoldoutGroup : PropertyGroupAttribute
    {
        public string name = "";

        public FoldoutGroup(string name)
        {
            this.name = name.Replace("/", "-");
        }
    }

    /// <summary>
    ///     Draw an information above the property in Inpector
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class InfoBox : PropertyGroupAttribute
    {
        public string info = "";

        public InfoBox(string info)
        {
            this.info = info;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class MinMax : PropertyGroupAttribute
    {
        public float max;
        public float min;

        public MinMax(float _min, float _max)
        {
            min = _min;
            max = _max;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ProgressBar : PropertyGroupAttribute
    {
        public int max;
        public int min;
        public string title;

        public ProgressBar(string _title, float _min, float _max)
        {
            min = Mathf.RoundToInt(_min);
            max = Mathf.RoundToInt(_max);
            title = _title;
        }
    }

    /// <summary>
    ///     Group the Items of the same flag
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class HorizontalGroup : PropertyGroupAttribute
    {
        public string name = "";

        public HorizontalGroup(string name)
        {
            this.name = name.Replace("/", "-");
        }

        public HorizontalGroup()
        {
            name = "";
        }
    }

    /// <summary>
    ///     Group the Items of the same flag
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BoxGroup : PropertyGroupAttribute
    {
        public string name = "";

        public BoxGroup(string name)
        {
            this.name = name.Replace("/", "-");
        }

        public BoxGroup(string name, bool center)
        {
            this.name = name.Replace("/", "-");
        }

        public BoxGroup()
        {
            name = "";
        }
    }

    /// <summary>
    ///     Only draw the property if the condition is positive.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ShowIf : PropertyGroupAttribute
    {
        public object condition;
        public string parameter = "";

        public ShowIf(string parameter)
        {
            this.parameter = parameter;
            condition = true;
        }

        public ShowIf(string parameter, object condition)
        {
            this.parameter = parameter;
            this.condition = condition;
        }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class Search : PropertyGroupAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UseSearch : PropertyGroupAttribute
    {
    }

    /// <summary>
    ///     Only draw the property if the condition is positive.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class EnableIf : PropertyGroupAttribute
    {
        public object condition;
        public string parameter = "";

        public EnableIf(string parameter)
        {
            this.parameter = parameter;
            condition = true;
        }

        public EnableIf(string parameter, object condition)
        {
            this.parameter = parameter;
            this.condition = condition;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class Doc : PropertyGroupAttribute
    {
        public string url;

        public Doc(string url)
        {
            this.url = url;
        }
    }

    /// <summary>
    ///     Only draw the property if the condition is positive.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DisableIf : PropertyGroupAttribute
    {
        public object condition;
        public string parameter = "";

        public DisableIf(string parameter)
        {
            this.parameter = parameter;
            condition = true;
        }

        public DisableIf(string parameter, object condition)
        {
            this.parameter = parameter;
            this.condition = condition;
        }
    }


    /// <summary>
    ///     Do not draw property if condition is positive
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public class HideIf : PropertyGroupAttribute
    {
        public object condition;
        public string parameter = "";

        public HideIf(string parameter)
        {
            this.parameter = parameter;
            condition = true;
        }

        public HideIf(string parameter, object condition)
        {
            this.parameter = parameter;
            this.condition = condition;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IgnoreAttributes : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UseAttributes : Attribute
    {
    }

    #endregion

#if UNITY_EDITOR
    public class AddButtonSelection : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - 30, position.height), property,
                label);
            if (GUI.Button(new Rect(position.width + 20, position.y, 20, position.height), "I"))
            {
                var select = new SelectItemEditor();
                select.Init(property);
            }

            EditorGUI.EndProperty();
        }
    }

    public class SelectItemEditor : EditorWindow
    {
        // Add menu named "My Window" to the Window menu
        public void Init(SerializedProperty property)
        {
            ActiveEditorTracker.sharedTracker.isLocked = true;

            Selection.activeObject = property.objectReferenceValue;
            // Retrieve the existing Inspector tab, or create a new one if none is open
            var inspectorWindow = GetWindow(typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow"));
            // Get the size of the currently window
            var size = new Vector2(inspectorWindow.position.width, inspectorWindow.position.height);
            // Clone the inspector tab (optionnal step)
            inspectorWindow = Instantiate(inspectorWindow);
            // inspectorWindow.ShowPopup();
            // Set min size, and focus the window
            inspectorWindow.minSize = size;
            inspectorWindow.Focus();
        }
    }

#endif

    public static class Helpers
    {
        public static IEnumerable<FieldInfo> GetAllFields(this Type type)
        {
            if (type == null) return Enumerable.Empty<FieldInfo>();

            var flags = BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Static |
                        BindingFlags.Instance |
                        BindingFlags.DeclaredOnly;

            return type.GetFields(flags).Union(GetAllFields(type.BaseType));
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            if (type == null) return Enumerable.Empty<PropertyInfo>();

            var flags = BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Static |
                        BindingFlags.Instance |
                        BindingFlags.DeclaredOnly;

            return type.GetProperties(flags).Union(GetAllProperties(type.BaseType));
        }
    }
}

public class ScriptableObjectId : PropertyAttribute { }

public class UniqueScriptableObject : ScriptableObject {
    [ScriptableObjectId] 
    public int Id = 0;
}