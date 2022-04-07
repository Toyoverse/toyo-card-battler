using System;
using Card;
using Card.CardPile;
using Extensions;
using UnityEngine;

namespace PlayerHand
{
    [RequireComponent(typeof(IPlayerHand))]
    public class PlayerHandSorter : MonoBehaviour
    {
        private const int offsetZ = -1;
        private ICardPile PlayerHand { get; set; }

        private void Awake()
        {
            PlayerHand = GetComponent<IPlayerHand>();
            CardRenderer = CardPrefab.GetComponentsInChildren<SpriteRenderer>()[0];
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
            if (cards == null || cards.Length <= 0) throw new ArgumentNullException("Can't sort empty card list");

            var _layerZ = 0;
            var _index = 0;
            var _cardsLenght = cards.Length;
            const int _spacing = 0;
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

        private void Bend(ICard card, int index, int cardsLenght, ref float offsetX, int spacing)
        {
            //Todo Transfer to CardData
            const int _fullAngle = -20;
            const float _height = 0.12f;

            const int _rotationSpeed = 20;
            const int _rotationSpeedEnemy = 500;

            const int _movementSpeed = 4;

            var _anglePerCard = _fullAngle / cardsLenght;
            var _firstAngle = CalcFirstAngle(_fullAngle);

            var pivotLocationFactor = pivot.CloserEdge(Camera.main, Screen.width, Screen.height);

            var angleTwist = (_firstAngle + index * _anglePerCard) * pivotLocationFactor;
            var xPos = offsetX + CardWidth / 2;
            var yDistance = Mathf.Abs(angleTwist) * _height;
            var yPos = pivot.position.y - yDistance * pivotLocationFactor;

            if (!card.IsDragging && !card.IsHovering)
            {
                var zAxisRot = pivotLocationFactor == 1 ? 0 : 180;
                var rotation = new Vector3(0, 0, angleTwist - zAxisRot);
                var position = new Vector3(xPos, yPos, card.transform.position.z);

                var rotSpeed = card.IsPlayer ? _rotationSpeed : _rotationSpeedEnemy;

                card.RotateTo(rotation, rotSpeed);
                card.MoveTo(position, _movementSpeed);
            }

            //increment offset
            offsetX += CardWidth + spacing;
        }

        private static float CalcFirstAngle(float fullAngle)
        {
            var magicMathFactor = 0.1f;
            return -(fullAngle / 2) + fullAngle * magicMathFactor;
        }

        private float CalcHandWidth(int quantityOfCards, int spacing)
        {
            var widthCards = quantityOfCards * CardWidth;
            var widthSpacing = (quantityOfCards - 1) * spacing;
            return widthCards + widthSpacing;
        }

        #region Fields and Properties

        [SerializeField] private CardData CardData;

        [SerializeField] [Tooltip("The Card Prefab")]
        private GameObject CardPrefab;

        [SerializeField] [Tooltip("Transform used as anchor to position the cards.")]
        private Transform pivot;

        private SpriteRenderer CardRenderer { get; set; }
        private float CardWidth => CardRenderer.bounds.size.x;

        #endregion
    }
}