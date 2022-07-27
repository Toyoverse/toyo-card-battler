using ServiceLocator;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public UiController optionsUiControl;
    public UiController mainMenuUiControl;
    
    public void Play()
    {
        Locator.GetSceneControl().LoadSceneAsync(1);
    }

    public void Options(bool on)
    {
        if (optionsUiControl != null)
        {
            optionsUiControl.EnableOrDisable(on);
            mainMenuUiControl.EnableOrDisable(!on);
        }
        else
        {
            Debug.LogError("Error: Options UIDocument is null.");
        }
    }
    
    public void QuitGame()
    {
        if (Application.isEditor)
        {
            //EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
