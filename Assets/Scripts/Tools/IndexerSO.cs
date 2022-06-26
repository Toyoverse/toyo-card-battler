using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(fileName = "IndexerSO", menuName = "ScriptableObject/IndexerSO")]
    public class IndexerSO : ScriptableObject
    {
        [ReadOnly]
        public long CardIndex = 0;
    }
}