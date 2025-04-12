using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    public static event Action<string> OnSceneLoaded;

    private string sceneToLoadAfterLoading;

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
        if (IsSceneLoaded(sceneName))
        {
            Debug.LogWarning($"[SceneManager] Scene '{sceneName}' is already loaded.");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName, false));
    }

    public void LoadSceneWithLoading(string targetSceneName)
    {
        sceneToLoadAfterLoading = targetSceneName;
        StartCoroutine(LoadSceneAsync("LoadingScene", false));
    }

    public string GetTargetSceneToLoad()
    {
        return sceneToLoadAfterLoading;
    }

    public void LoadAdditiveScene(string sceneName)
    {
        if (IsSceneLoaded(sceneName))
        {
            Debug.LogWarning($"[SceneManager] Scene '{sceneName}' is already loaded additively.");
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName, true));
    }

    public void UnloadScene(string sceneName)
    {
        if (!IsSceneLoaded(sceneName))
        {
            Debug.LogWarning($"[SceneManager] Tried to unload scene '{sceneName}' which isn't loaded.");
            return;
        }

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
    }

    public IEnumerator LoadSceneAsync(string sceneName, bool additive)
    {
        AsyncOperation operation;

        if (additive)
            operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        else
            operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log($"[SceneManager] Loading '{sceneName}'... {progress * 100:F0}%");
            yield return null;
        }

        Debug.Log($"[SceneManager] Scene '{sceneName}' loaded successfully.");
        OnSceneLoaded?.Invoke(sceneName);
    }

    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}
