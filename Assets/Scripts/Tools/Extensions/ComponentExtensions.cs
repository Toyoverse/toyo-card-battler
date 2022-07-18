using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools.Extensions
{
    /// <summary>
    ///     Extension methods for UnityEngine.Component.
    ///     Ref: https://github.com/mminer/unity-extensions/blob/master/ComponentExtensions.cs
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        ///     Attaches a component to the given component's game object.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Newly attached component.</returns>
        public static T AddComponent<T>(this Component component) 
            where T : Component => component.gameObject.AddComponent<T>();

        /// <summary>
        ///     Gets a component attached to the given component's game object.
        ///     If one isn't found, a new one is attached and returned.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Previously or newly attached component.</returns>
        public static T GetOrAddComponent<T>(this Component component) 
            where T : Component => component.GetComponent<T>() ?? component.AddComponent<T>();

        /// <summary>
        ///     Checks whether a component's game object has a component of type T attached.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>True when component is attached.</returns>
        public static bool HasComponent<T>(this Component component) 
            where T : Component => component.GetComponent<T>() != null;
        
        public static T LazyGetComponent<T>(this Component comp, ref T backingField) => backingField ??= comp.GetComponent<T> ();
   
        public static T LazyGetComponentInChildren<T> (this Component comp, ref T backingField) 
            where T : Component => backingField ??= comp.GetComponentInChildren<T> ();
 
        public static T LazyGetComponentInParent<T> (this Component comp, ref T backingField) 
            where T : Component => backingField ??=  comp.GetComponentInParent<T> ();
 
        public static T LazyNew<T> (this Component _, ref T backingField) 
            where T : new() => backingField ??= new T();
        
        public static T LazyFindOfType<T>(this Component _, ref T backingField) 
            where T : class => backingField ??= Object.FindObjectsOfType<MonoBehaviour>().OfType<T>().FirstOrDefault(); 
    }
}