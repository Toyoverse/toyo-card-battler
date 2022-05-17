using System.Collections.Generic;
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