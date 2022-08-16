using System;
using Player;
using TMPro;
using UnityEngine;

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

        #endregion
    }
}
