using System;
using Tools;
using UnityEngine;

namespace DefaultNamespace.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "GlobalCardData", menuName = "ScriptableObject/GlobalCardData", order = 1)]
    public class GlobalCardDataSO : ScriptableObject
    {
        #region Defaults
        
        [Button]
        public void SetDefaults()
        {
            disabledAlpha = 0.5f;

            hoverHeight = 1;
            hoverRotation = false;
            hoverScale = 1.3f;
            hoverSpeed = 15f;

            height = 0.12f;
            spacing = -2;
            fullAngle = -20;

            rotationSpeedEnemy = 500;
            rotationSpeed = 20;
            movementSpeed = 4;
            scaleSpeed = 7;

            startSizeWhenDraw = 0.05f;
            discardedSize = 0.5f;

            layerToRenderNormal = 0;
            layerToRenderTop = 1;
        }

        #endregion

        #region Disable

        public float DisabledAlpha
        {
            get => disabledAlpha;
            set => disabledAlpha = value;
        }
        [Header("Disable")] [Tooltip("How a card fades when disabled.")] [SerializeField] [Range(0.1f, 1)]
        float disabledAlpha;

        #endregion
        
        #region Hover
        public bool HoverRotation
        {
            get => hoverRotation;
            set => hoverRotation = value;
        }
        [SerializeField] [Tooltip("Whether the hovered card keep its rotation.")]
        bool hoverRotation;

        public float HoverScale
        {
            get => hoverScale;
            set => hoverScale = value;
        }
        [SerializeField] [Tooltip("How much a hovered card scales.")] [Range(0.9f, 2f)]
        float hoverScale;

        public float HoverSpeed
        {
            get => hoverSpeed;
            set => hoverSpeed = value;
        }
        [SerializeField] [Range(0, 25)] 
        float hoverSpeed;
        
        public float HoverHeight
        {
            get => hoverHeight;
            set => hoverHeight = value;
        }
        [Header("Hover")] [SerializeField] [Tooltip("How much the card will go upwards when hovered.")] [Range(0, 4)]
        float hoverHeight;
        

        #endregion

        #region Bend

        public float Height
        {
            get => height;
            set => height = value;
        }
        [Header("Bend")] [SerializeField] [Tooltip("Height factor between two cards.")] [Range(0f, 1f)]
        float height;

        public float Spacing
        {
            get => spacing;
            set => spacing = -value;
        }
        [SerializeField] [Tooltip("Amount of space between the cards on the X axis")] [Range(0f, -5f)]
        float spacing;

        public float FullAngle
        {
            get => fullAngle;
            set => fullAngle = value;
        }
        [SerializeField] [Range(-50, 50)]
        float fullAngle;


        #endregion


        #region Movement
        

        
        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }
        [Header("Rotation")] [SerializeField] [Range(0, 60)] [Tooltip("Speed of a card while it is rotating")]
        float rotationSpeed;

        public float RotationSpeedEnemy
        {
            get => rotationSpeedEnemy;
            set => rotationSpeedEnemy = value;
        }
        [SerializeField] [Range(0, 1000)] [Tooltip("Speed of a card while it is rotating for player 2")]
        float rotationSpeedEnemy;

        public float MovementSpeed
        {
            get => movementSpeed;
            set => movementSpeed = value;
        }
        [Header("Movement")] [SerializeField] [Range(0, 15)] [Tooltip("Speed of a card while it is moving")]
        float movementSpeed;

        
        public float ScaleSpeed
        {
            get => scaleSpeed;
            set => scaleSpeed = value;
        }
        [Header("Scale")] [SerializeField] [Range(0, 15)] [Tooltip("Speed of a card while it is scaling")]
        float scaleSpeed;

        #endregion

        #region Draw Discard

        public float StartSizeWhenDraw
        {
            get => startSizeWhenDraw;
            set => startSizeWhenDraw = value;
        }
        [Header("Draw")] [SerializeField] [Range(0, 1)] [Tooltip("Scale when draw the card")]
        float startSizeWhenDraw;

        public float DiscardedSize => discardedSize;
        [Header("Discard")] [SerializeField] [Range(0, 1)] [Tooltip("Scale when discard the card")]
        float discardedSize;

        #endregion
        
        
        public int LayerToRenderNormal
        {
            get => layerToRenderNormal;
            set => layerToRenderNormal = value;
        }
        [Header("Layer")] [SerializeField] [Range(0, 5)] 
        int layerToRenderNormal;
        
        public int LayerToRenderTop
        {
            get => layerToRenderTop;
            set => layerToRenderTop = value;
        }
        [SerializeField] [Range(0, 5)] 
        int layerToRenderTop;
        
    }
}