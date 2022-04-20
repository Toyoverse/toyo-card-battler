using System;
using Card;
using Card.CardPile;
using DefaultNamespace;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerHand
{
    [RequireComponent(typeof(IPlayerHand))]
    public class PlayerHandSorter : MonoBehaviour
    {
        private int offsetZ => GlobalConfig.Instance.globalCardDataSO.offsetZ;
        
        private ICardPile PlayerHand { get; set; }

        private void Awake()
        {
            PlayerHand = GetComponent<IPlayerHand>();
            CardImage = GlobalConfig.Instance.cardDefaultPrefab.GetComponentsInChildren<SpriteRenderer>()[0];
        }

        private void OnEnable()
        {
            PlayerHand.OnPileChanged += Sort;
        }

        private void OnDisable()
        {
            PlayerHand.OnPileChanged -= Sort;
        }

        private void Sort(ICard[] cards)
        {
            if (cards == null ) throw new ArgumentNullException("Can't sort empty card list");
            if (cards.Length <= 0) return;

            var _layerZ = 0;
            var _index = 0;
            var _cardsLenght = cards.Length;
            var _spacing = GlobalConfig.Instance.globalCardDataSO.Spacing;
            var _handWidth = CalcHandWidth(_cardsLenght, _spacing);
            var _offsetX = pivot.position.x - _handWidth / 2;

            foreach (var _card in cards)
            {
                SortOffsetZ(_card, ref _layerZ);
                Bend(_card, _index, _cardsLenght, ref _offsetX, _spacing);
                _index++;
            }
        }

        private void SortOffsetZ(ICard card, ref int layerZ)
        {
            var _localCardPosition = card.transform.position;
            _localCardPosition.z = layerZ;
            card.transform.position = _localCardPosition;
            layerZ += offsetZ;
        }

        private void Bend(ICard card, int index, int cardsLenght, ref float offsetX, float spacing)
        {
            var _anglePerCard = GlobalCardData.FullAngle / cardsLenght;
            var _firstAngle = CalcFirstAngle(GlobalCardData.FullAngle);

            var pivotLocationFactor = pivot.CloserEdge(Camera.main, Screen.width, Screen.height);

            var angleTwist = (_firstAngle + index * _anglePerCard) * pivotLocationFactor;
            var xPos = offsetX + CardWidth / 2;
            var yDistance = Mathf.Abs(angleTwist) * GlobalCardData.Height;
            var yPos = pivot.position.y - yDistance * pivotLocationFactor;

            if (!card.IsDragging && !card.IsHovering)
            {
                var zAxisRot = pivotLocationFactor == 1 ? 0 : 180;
                var rotation = new Vector3(0, 0, angleTwist - zAxisRot);
                var position = new Vector3(xPos, yPos, card.transform.position.z);

                var rotSpeed = card.IsPlayer ? GlobalCardData.RotationSpeed : GlobalCardData.RotationSpeedEnemy;

                card.RotateTo(rotation, rotSpeed);
                card.MoveTo(position, GlobalCardData.MovementSpeed);
            }

            //increment offset
            offsetX += CardWidth + spacing;
        }

        private static float CalcFirstAngle(float fullAngle)
        {
            var magicMathFactor = 0.1f;
            return -(fullAngle / 2) + fullAngle * magicMathFactor;
        }

        private float CalcHandWidth(int quantityOfCards, float spacing)
        {
            var widthCards = quantityOfCards * CardWidth;
            var widthSpacing = (quantityOfCards - 1) * spacing;
            return widthCards + widthSpacing;
        }

        #region Fields and Properties

        private Transform pivot => GlobalConfig.Instance.handPivot;

        private SpriteRenderer CardImage { get; set; }
        private float CardWidth => CardImage.bounds.size.x;

        #endregion
    }
}