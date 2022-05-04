using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneControl : MonoBehaviour
{
    public UIDocument loadingUiDoc;
    private Label loadingText;
    public static SceneControl Instance;
    public bool init = false;

    private void Awake()
    {
        if (Instance != null && Instance.init)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    }

    private void Init()
    {
        if (loadingUiDoc == null)
        {
            loadingUiDoc = this.gameObject.GetComponent<UIDocument>();
        }
        var root = loadingUiDoc.rootVisualElement;
        //loadingText = root.Query<Label>().First();
        init = true;
    }

    public static void LoadSceneAsync(int sceneIndex)
    {
        if (Instance == null)
        {
            Instance = InstancePrefab().GetComponent<SceneControl>();
        }
        Instance.StartCoroutine(LoadAsync(sceneIndex));
    }

    //Method overload to call scene by name
    public static void LoadSceneAsync(string sceneName)
    {
        if (Instance == null)
        {
            Instance = InstancePrefab().GetComponent<SceneControl>();
        }
        Instance.StartCoroutine(LoadAsync(sceneName));
    }

    static IEnumerator LoadAsync(int sceneIndex)
    {
        while (!Instance.init)
        {
            yield return null;
        }
        
        Instance.loadingUiDoc.enabled = true;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!asyncOperation.isDone)
        {
            //Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            //Instance.loadingText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            yield return null;
        }
        
        yield return new WaitForFixedUpdate(); /*WaitForSeconds(3);*/
        Instance.loadingUiDoc.enabled = false;
    }
    
    //Overload to call scene by name
    static IEnumerator LoadAsync(string sceneName)
    {
        while (!Instance.init)
        {
            yield return null;
        }
        
        Instance.loadingUiDoc.enabled = true;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            Instance.loadingText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            yield return null;
        }

        yield return new WaitForFixedUpdate();
        Instance.loadingUiDoc.enabled = false;
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
