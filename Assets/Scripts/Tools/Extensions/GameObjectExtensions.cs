using UnityEngine;

namespace Extensions
{
    /// <summary>
    ///     Extension methods for UnityEngine.GameObject.
    ///     Ref: https://github.com/mminer/unity-extensions/blob/master/GameObjectExtensions.cs
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        ///     Gets a component attached to the given game object.
        ///     If one isn't found, a new one is attached and returned.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <returns>Previously or newly attached component.</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) 
            where T : Component => gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();

        /// <summary>
        ///     Checks whether a game object has a component of type T attached.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <returns>True when component is attached.</returns>
        public static bool HasComponent<T>(this GameObject gameObject) 
            where T : Component => gameObject.GetComponent<T>() != null;
            
        public static TComponent AddComponent<TComponent, TFirstArgument, TSecondArgument>
            (this GameObject gameObject, TFirstArgument firstArgument, TSecondArgument secondArgument)
            where TComponent : MonoBehaviour
        {
            Arguments<TFirstArgument, TSecondArgument>.First = firstArgument;
            Arguments<TFirstArgument, TSecondArgument>.Second = secondArgument;
         
            var _component = gameObject.AddComponent<TComponent>();
 
            Arguments<TFirstArgument, TSecondArgument>.First = default;
            Arguments<TFirstArgument, TSecondArgument>.Second = default;
 
            return _component;
        }
    }
    
    public static class Arguments<TFirstArgument, TSecondArgument>
    {
        public static TFirstArgument First { get; internal set; }
        public static TSecondArgument Second { get; internal set; }
    }
    
    /*
     * Reference of pattern
    public class InteractObject : MonoBehaviour
    {
        private bool rayHit;
        private string objectName;
 
        public InteractObject(bool rayHit, string objectName)
        {
            this.rayHit = rayHit;
            this.objectName = objectName;
            Debug.Log(objectName);
        }
 
        public InteractObject() : this(Arguments<bool, string>.First, Arguments<bool, string>.Second) { }
    }
    */

}