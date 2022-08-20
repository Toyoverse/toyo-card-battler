using System;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Card.QueueSystem
{
    public class CardQueueSystemPresenter : MonoBehaviour
    {
        public GameObject playerComboObj;
        public GameObject enemyComboObj;
        
        public TextMeshProUGUI playerQueueSize;
        public TextMeshProUGUI enemyQueueSize;
        public TextMeshProUGUI currentCardDuration;
        public TextMeshProUGUI playerCurrentCombo;
        public TextMeshProUGUI enemyCurrentCombo;

        public RawImage playerCardStatus;
        public RawImage enemyCardStatus;

        private void OnEnable()
        {
            OnUpdateUI += UpdateUI;
            OnActivateComboUI += ActiveComboUI;
            OnCardStatusChange += SetCardStatus;
        }

        private void OnDisable()
        {
            OnUpdateUI -= UpdateUI;
            OnActivateComboUI -= ActiveComboUI;
            OnCardStatusChange -= SetCardStatus;
        }

        private void SetCardStatus(bool isPlayer, CARD_STATUS cardStatus)
        {
            var _color = cardStatus switch
            {
                CARD_STATUS.BUFFER => Color.gray,
                CARD_STATUS.ACTIVE => Color.blue,
                CARD_STATUS.HIT => Color.red,
                CARD_STATUS.DISABLE => Color.clear,
                _ => throw new ArgumentOutOfRangeException(nameof(cardStatus), cardStatus, null)
            };
            if (isPlayer)
                playerCardStatus.DOColor(_color, 0.0f);
            else
                enemyCardStatus.DOColor(_color, 0.0f);
        }

        private void UpdateUI(string playerQueueSize, string enemyQueueSize,
            string playerCurrentCombo, string enemyCurrentCombo, string currentCardDuration)
        {
            if (FusionLauncher.IsServer)
            {
                this.playerQueueSize.text = playerQueueSize;
                this.enemyQueueSize.text = enemyQueueSize;
                this.playerCurrentCombo.text = playerCurrentCombo;
                this.enemyCurrentCombo.text = enemyCurrentCombo;
            }
            else
            {
                this.playerQueueSize.text = enemyQueueSize;
                this.enemyQueueSize.text = playerQueueSize;
                this.playerCurrentCombo.text = enemyCurrentCombo;
                this.enemyCurrentCombo.text = playerCurrentCombo;
            }

            this.currentCardDuration.text = currentCardDuration;
        }
        
        private void ActiveComboUI(PlayerNetworkManager _playerNetworkManager)
        {
            if (FusionLauncher.IsServer)
            {
                if (_playerNetworkManager.PlayerCurrentCombo > 0)
                {
                    if (!playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(true);
                }
                else
                {
                    if(playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(false);
                }

                if (_playerNetworkManager.EnemyCurrentCombo > 0)
                {
                    if(!enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(true);
                }
                else
                {
                    if(enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(false);
                }
            }
            else
            {
                if (_playerNetworkManager.PlayerCurrentCombo > 0)
                {
                    if (!enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(true);
                }
                else
                {
                    if(enemyComboObj.activeInHierarchy)
                        enemyComboObj.SetActive(false);
                }

                if (_playerNetworkManager.EnemyCurrentCombo > 0)
                {
                    if(!playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(true);
                }
                else
                {
                    if(playerComboObj.activeInHierarchy)
                        playerComboObj.SetActive(false);
                }
            }
        }

        #region Events

        public Action<string, string, string, string, string> OnUpdateUI { get; set; }
        public Action<PlayerNetworkManager> OnActivateComboUI { get; set; }
        public Action<bool, CARD_STATUS> OnCardStatusChange { get; set; }

        public class UpdateCardStatusSignal
        {
            public bool IsPlayer = false;
            public CARD_STATUS CardStatus = CARD_STATUS.BUFFER;
            
        }

        #endregion
    }
}
