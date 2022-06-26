using System;
using Scriptable_Objects;
using Sirenix.OdinInspector;
using UnityEngine;

    [Serializable]
    public class GlobalConfig : Singleton<GlobalConfig>
    {
        [FoldoutGroup("Card Parameters")] public GlobalCardDataSO globalCardDataSO;
        [FoldoutGroup("Card Parameters")] public CombatConfigSO CombatConfigSO;
        [FoldoutGroup("Card Parameters")] public DeckDatabaseSO DeckDatabaseSo;
        
        
        [FoldoutGroup("Card Transforms")] public Transform gameView;
        [FoldoutGroup("Card Transforms")] public Transform deckPosition;
        [FoldoutGroup("Card Transforms")] public Transform handPivot;
        [FoldoutGroup("Card Transforms")] public Transform graveyardPosition;

        [FoldoutGroup("Game Parameters")] public GameObject UI;
        [FoldoutGroup("Game Parameters")] public GameObject PlayerUI;
        [FoldoutGroup("Game Parameters")] public GameObject EnemyUI;
        
        [FoldoutGroup("Temp Settings")] public int maxAP;
        [FoldoutGroup("Temp Settings")] public float timeForApRegen = 20.0f;
        
        [FoldoutGroup("Cheats")] public bool IgnoreAPCost;
        [FoldoutGroup("Cheats")] public bool IgnoreDamageCalculations;

        [FoldoutGroup("Leveling")] public PlayerXPConfigSO playerXpConfigSo;
        [FoldoutGroup("Leveling")] public ToyoPartsXPConfigSO partXpConfigSo;
        [FoldoutGroup("Leveling")] public RankedXPConfigSO rankingXpConfigSo;
        
        
        [FoldoutGroup("Match")] public NormalMatchConfigSO normalMatchConfigSo;
        [FoldoutGroup("Match")] public RankedMatchConfigSO rankedMatchConfigSo;

        internal BattleReferences battleReferences;
        
        void Awake()
        {
            GlobalCardData.Initialize(globalCardDataSO);
            battleReferences = gameObject.GetComponent<BattleReferences>();

        }
    }

    public static class GlobalCardData
    {
        public static float DisabledAlpha;
        public static int LayerToRenderNormal;
        public static int LayerToRenderTop;
        public static float DiscardedSize;
        public static float ScaleSpeed;
        public static float RotationSpeed;
        public static float RotationSpeedEnemy;
        public static float StartSizeWhenDraw;
        public static float HoverHeight;
        public static float HoverSpeed;
        public static float HoverScale;
        public static bool HoverRotation;
        public static float FullAngle;
        public static float Height;
        public static float MovementSpeed;


        public static void Initialize(GlobalCardDataSO _cardDataSO)
        {
            DisabledAlpha = _cardDataSO.DisabledAlpha;
            LayerToRenderNormal = _cardDataSO.LayerToRenderNormal; 
            LayerToRenderTop = _cardDataSO.LayerToRenderTop;
            DiscardedSize = _cardDataSO.DiscardedSize;
            ScaleSpeed = _cardDataSO.ScaleSpeed;
            RotationSpeed = _cardDataSO.RotationSpeed;
            RotationSpeedEnemy = _cardDataSO.RotationSpeedEnemy;
            StartSizeWhenDraw = _cardDataSO.StartSizeWhenDraw;
            HoverHeight = _cardDataSO.HoverHeight;
            HoverSpeed = _cardDataSO.HoverSpeed;
            HoverScale = _cardDataSO.HoverScale;
            HoverRotation = _cardDataSO.HoverRotation;
            FullAngle = _cardDataSO.FullAngle;
            Height = _cardDataSO.Height;
            MovementSpeed = _cardDataSO.MovementSpeed;
        }
    }
