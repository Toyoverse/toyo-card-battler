using FusionExamples.Tanknarok;
using PlayerHand;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameLauncher _gameLauncher;
    private UiController _uiController;
    
    private void Start()
    {
        _gameLauncher = FindObjectOfType<GameLauncher>();
        _uiController = this.GetComponent<UiController>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.P)) //TODO: Move this for correct local
        {
            _uiController.EnableOrDisable(!_uiController.UiDoc.enabled);
        }
    }

    public void CallMainMenu()
    {
        SceneControl.LoadSceneAsync(0);
    }

    public void Restart()
    {
        SceneControl.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    void StartGame() => StartCoroutine(FindObjectOfType<PlayerHandUtils>()?.DrawFirstHand()); 

    public void Host()
    {
        _gameLauncher.OnHostOptions();
        _gameLauncher.OnEnterRoom();
        StartGame();
        _uiController.EnableOrDisable(false);
    }

    public void Join()
    {
        _gameLauncher.OnJoinOptions();
        _gameLauncher.OnEnterRoom();
        StartGame();
        _uiController.EnableOrDisable(false);
    }

    public void Shared()
    {
        _gameLauncher.OnSharedOptions();
        _gameLauncher.OnEnterRoom();
        StartGame();
        _uiController.EnableOrDisable(false);
    }
}
