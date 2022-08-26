using System;
using System.Collections;
using ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneControl : MonoBehaviour
{
    private UIDocument _loadingUiDoc;
    private Label _loadingText;
    private bool _init;

    private void Awake()
    {
        Init();
        Locator.Provide(this);
    }

    private void Init()
    {
        if (_loadingUiDoc == null)
        {
            _loadingUiDoc = this.gameObject.GetComponent<UIDocument>();
            var _ = _loadingUiDoc?.rootVisualElement;
        }
        //loadingText = root.Query<Label>().First();
        _init = true;
    }

    public void LoadSceneAsync(int sceneIndex)
    {
        // if (Instance == null)
        // {
        //     Instance = InstancePrefab().GetComponent<SceneControl>();
        // }
        // Instance.StartCoroutine(LoadAsync(sceneIndex));
        
        StartCoroutine(LoadAsync(sceneIndex));
    }

    //Method overload to call scene by name
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        while (!_init)
        {
            yield return null;
        }
        if(_loadingUiDoc)
            _loadingUiDoc.enabled = true;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!asyncOperation.isDone)
        {
            //Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            //Instance.loadingText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            yield return null;
        }
        
        yield return new WaitForFixedUpdate(); /*WaitForSeconds(3);*/
        if(_loadingUiDoc)
            _loadingUiDoc.enabled = false;
    }
    
    //Overload to call scene by name
    IEnumerator LoadAsync(string sceneName)
    {
        while (!_init)
        {
            yield return null;
        }
        
        _loadingUiDoc.enabled = true;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            _loadingText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            yield return null;
        }

        yield return new WaitForFixedUpdate();
        _loadingUiDoc.enabled = false;
    }

    static GameObject InstancePrefab()
    {
        var loadedObject = Resources.Load("SceneControl");
        if (loadedObject == null)
        {
            Debug.LogError("No file found: SceneControl.");
        }
        else
        {
            return (GameObject)Instantiate(loadedObject);
        }

        return null;
    }
}
