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
        }

        private void OnDisable()
        {
            OnUpdateUI -= UpdateUI;
            OnActivateComboUI -= ActiveComboUI;
        }

        private void SetCardBuffer(bool isPlayer) => SetCardStatus(isPlayer, Color.gray);
        
        private void SetCardActive(bool isPlayer) => SetCardStatus(isPlayer, Color.blue);
        
        private void SetCardHit(bool isPlayer) => SetCardStatus(isPlayer, Color.red);
        
        private void SetCardStatus(bool isPlayer, Color color) //Change status dot colors to simulate validation process
        {
            if (isPlayer)
                playerCardStatus.DOColor(color, 0.0f);
            else
                enemyCardStatus.DOColor(color, 0.0f);
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
        
        public class UpdateCardStatusSignal
        {
            public bool IsPlayer;
            public CARD_STATUS CardStatus;
            
            
        }

        #endregion
    }
}
