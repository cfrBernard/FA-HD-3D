using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
        SceneManager.OnSceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
        SceneManager.OnSceneLoaded -= HandleSceneLoaded;
    }

    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                // UI
                break;
            case GameState.Settings:
                // UI
                break;
            case GameState.Playing:
                // UI
                break;
        }
    }

    private void HandleSceneLoaded(string sceneName)
    {
        switch (sceneName)
        {
            case SceneNames.SettingsMenu:
                Debug.Log("[UIManager] Settings Menu loaded.");
                // UI
                break;
            case SceneNames.MainMenu:
                Debug.Log("[UIManager] Main Menu loaded.");
                // UI
                break;
        }
    }

    public void OnPlayGame()
    {
        Debug.Log("START");
        GameManager.Instance.SetGameState(GameState.Playing);
    }

    public void OnOpenSettings()
    {
        Debug.Log("SETTINGS");
        SceneManager.Instance.LoadAdditiveScene(SceneNames.SettingsMenu);
    }

    public void OnExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
