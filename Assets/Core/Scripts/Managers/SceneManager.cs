using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, false));
    }

    public void LoadAdditiveScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, true));
    }

    private IEnumerator LoadSceneAsync(string sceneName, bool additive)
    {
        AsyncOperation operation;

        if (additive)
        {
            
            // Loading the scene additively
            operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            // Classic loading of a new scene
            operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); 
            Debug.Log("Loading Progress: " + progress * 100 + "%");
            yield return null;
        }

        if (!additive)
        {
            // fade-in/out or other effects
            Debug.Log("Scene " + sceneName + " Loaded!");
        }
    }

    public void UnloadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
    }
}
