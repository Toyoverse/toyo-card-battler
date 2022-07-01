using UnityEngine;

namespace Tools.Patterns.MVP
{
    public class Presenter<T> : MonoBehaviour 
    {
        [SerializeField] public T Model;
    }
}