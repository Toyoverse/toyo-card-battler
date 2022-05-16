using System;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

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
    }
}