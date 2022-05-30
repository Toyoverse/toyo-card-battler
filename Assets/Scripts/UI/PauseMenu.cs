using System;
using FusionExamples.Tanknarok;
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
            _uiController.EnableOrDisable(!_uiController.UiDoc.enabled);
    }

    public void CallMainMenu() => SceneControl.LoadSceneAsync(0);

    public void Restart()
        => SceneControl.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    public void Host() => StartRoom(_gameLauncher.OnHostOptions);

    public void Join() => StartRoom(_gameLauncher.OnJoinOptions);

    public void Shared() => StartRoom(_gameLauncher.OnSharedOptions);

    public void Single() => StartRoom();

    private void StartRoom(Action roomType = null)
    {
        roomType?.Invoke();
        _gameLauncher.OnEnterRoom();
        _uiController.EnableOrDisable(false);
    }
}