using System;
using Globals;
using ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameLauncherModel _gameLauncherModel;
    private UiController _uiController;
    
    private void Start()
    {
        _gameLauncherModel = FindObjectOfType<GameLauncherModel>();
        _uiController = this.GetComponent<UiController>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.P)) //TODO: Move this for correct local
            _uiController.EnableOrDisable(!_uiController.UiDoc.enabled);
    }

    public void CallMainMenu() => Locator.GetSceneControl().LoadSceneAsync(0);

    public void Restart()
        => Locator.GetSceneControl().LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    public void Host() => StartRoom(_gameLauncherModel.OnHostOptions);

    public void Join() => StartRoom(_gameLauncherModel.OnJoinOptions);

    public void Shared() => StartRoom(_gameLauncherModel.OnSharedOptions);

    public void Single() => StartRoom();

    private void StartRoom(Action roomType = null)
    {
        roomType?.Invoke();
        _gameLauncherModel.OnEnterRoom();
        _uiController.EnableOrDisable(false);
    }
}