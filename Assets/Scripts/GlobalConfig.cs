using System;
using Tools;

namespace DefaultNamespace
{
    [Serializable]
    public class GlobalConfig : Singleton<GlobalConfig>
    {
        [FoldoutGroup("Card Parameters")] public CardData cardData;

        [FoldoutGroup("Card Parameters")] public float test;

        [FoldoutGroup("Card Parameters")] public float test2;
    }
}