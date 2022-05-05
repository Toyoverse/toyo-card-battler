using System;
using Tools;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "GlobalCardData", menuName = "ScriptableObject/GlobalCardData", order = 1)]
    public class GlobalCardDataSO : ScriptableObject
    {
        #region Defaults
        
        [Button]
        public void SetDefaults()
        {
            DisabledAlpha = 0.5f;

            HoverHeight = 1;
            HoverRotation = false;
            HoverScale = 1.3f;
            HoverSpeed = 15f;

            Height = 0.12f;
            Spacing = -2;
            FullAngle = -20;

            RotationSpeedEnemy = 500;
            RotationSpeed = 20;
            MovementSpeed = 4;
            ScaleSpeed = 7;

            StartSizeWhenDraw = 0.05f;
            DiscardedSize = 0.5f;

            LayerToRenderNormal = 0;
            LayerToRenderTop = 1;

            bondCardDamage = 1.0f;
            heavyCardSumFactor = 1.0f;

            heavyCardMultiplierFactor = 1.0f;
            
            fastCardSumFactor = 0.0f;

            fastCardMultiplierFactor = 1.0f;
            
            superCardSumFactor = 0.0f;

            superCardMultiplierFactor = 0.0f;

        }

        #endregion

        #region Disable

        [Header("Disable")] [Tooltip("How a card fades when disabled.")] [SerializeField] [Range(0.1f, 1)]
        public float DisabledAlpha;

        #endregion
        
        #region Hover
        [SerializeField] [Tooltip("Whether the hovered card keep its rotation.")]
        public bool HoverRotation;

        [SerializeField] [Tooltip("How much a hovered card scales.")] [Range(0.2f, 4f)]
        public float HoverScale;

        [SerializeField] [Range(0, 50)] 
        public float HoverSpeed;
        
        [Header("Hover")] [SerializeField] [Tooltip("How much the card will go upwards when hovered.")] [Range(0, 10)]
        public float HoverHeight;
        

        #endregion

        #region Bend

        [Header("Bend")] [SerializeField] [Tooltip("Height factor between two cards.")] [Range(0f, 1f)]
        public float Height;

        [SerializeField] [Tooltip("Amount of space between the cards on the X axis")] [Range(-20, 20f)]
        public float Spacing;

        [SerializeField] [Range(-50, 50)]
        public float FullAngle;


        #endregion


        #region Movement
        
        [Header("Rotation")] [SerializeField] [Range(0, 120)] [Tooltip("Speed of a card while it is rotating")]
        public float RotationSpeed;

        [SerializeField] [Range(0, 1000)] [Tooltip("Speed of a card while it is rotating for player 2")]
        public float RotationSpeedEnemy;

        [Header("Movement")] [SerializeField] [Range(0, 50)] [Tooltip("Speed of a card while it is moving")]
        public float MovementSpeed;

        [Header("Scale")] [SerializeField] [Range(0, 50)] [Tooltip("Speed of a card while it is scaling")]
        public float ScaleSpeed;

        #endregion

        #region Draw Discard

        [Header("Draw")] [SerializeField] [Range(0, 1)] [Tooltip("Scale when draw the card")]
        public float StartSizeWhenDraw;

        [Header("Discard")] [SerializeField] [Range(0, 1)] [Tooltip("Scale when discard the card")]
        public float DiscardedSize;

        #endregion
        
        [Header("Layer")] [SerializeField] [Range(-10, 10)] 
        public int LayerToRenderNormal;
        
        [SerializeField] [Range(-10, 10)] 
        public int LayerToRenderTop;
        
        [SerializeField] [Range(-5, 5)] 
        public int offsetZ = -1;
        
        [Header("CombatSettings")]
        [SerializeField] [Range(0.0f, 5.0f)] 
        public float bondCardDamage = -1;

        [Tooltip("Amount by which the COMBO is divided in the calculation of HEAVY ATTACK DAMAGE.")]
        [SerializeField] [Range(1, 20)] 
        public int comboSystemFactor = 5;
        
        [Tooltip("Value added to HEAVY ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float heavyCardSumFactor = 1.0f;

        [Tooltip("Multiplier in HEAVY ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float heavyCardMultiplierFactor = 1.0f;
        
        [Tooltip("Value added to FAST ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float fastCardSumFactor = 0.0f;

        [Tooltip("Multiplier in FAST ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float fastCardMultiplierFactor = 1.0f;
        
        [Tooltip("Value added to SUPER ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float superCardSumFactor = 0.0f;

        [Tooltip("Multiplier in SUPER ATTACK calculations.")]
        [SerializeField] [Range(0.0f, 10.0f)] 
        public float superCardMultiplierFactor = 1.0f;

        [Tooltip("")] //TODO: Finish adding the tooltips to the other variables.
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float criticalLuckFactor = 0.025f;

        [Tooltip("")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float criticalPrecisionFactor = 0.05f;
        
        [Tooltip("BASE counter attack chance (BEFORE Toyo stat calculation).")]
        [SerializeField] [Range(0.0f, 100.0f)] 
        public float baseCounterChance = 5.0f;
        
        [Tooltip("")]
        [SerializeField] [Range(0.0f, 100.0f)] 
        public float defenseInCritical = 0f;
        
        [Tooltip("")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float maxCriticalChance = 0.5f;
        
        [Tooltip("")]
        [SerializeField] [Range(1f, 10f)] 
        public float criticalDamageModifier = 2f;
        
        [Tooltip("")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float resistenceMultiplier = 0.1f;
        
        [Tooltip("")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float resilianceMultiplier = 0.1f;
        
        [Tooltip("ANALYSIS multiplier used to generate BLOCK CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float analysisMultiplier = 0.11f;
        
        [Tooltip("SPEED multiplier used to generate DODGE CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float speedMultiplier = 0.11f;
        
        [Tooltip("AGILITY multiplier used to generate DODGE CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float agilityDefMultiplier = 0.1f;
        
        [Tooltip("TECHNIQUE multiplier used to generate BLOCK CHANCE value.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float techDefMultiplier = 0.1f;
        
        [Tooltip("LUCK multiplier used to generate COUNTER ATTACK CHANCE value after dodge or block success.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float counterLuckFactor = 0.1f;
        
        [Tooltip("AGILITY multiplier used to generate COUNTER ATTACK CHANCE value after dodge success.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float counterAgilityMultiplier = 0.05f;
        
        [Tooltip("TECHNIQUE multiplier used to generate COUNTER ATTACK CHANCE value after blocker success.")]
        [SerializeField] [Range(0.0f, 1.0f)] 
        public float counterTechMultiplier = 0.05f;
    }
}