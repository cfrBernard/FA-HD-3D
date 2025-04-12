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
                // Afficher l'UI
                break;
            case GameState.Settings:
                // Afficher l'UI
                break;
            case GameState.Playing:
                // Afficher l'UI
                break;
        }
    }

    private void HandleSceneLoaded(string sceneName)
    {
        switch (sceneName)
        {
            case SceneNames.SettingsMenu:
                Debug.Log("[UIManager] Settings Menu loaded.");
                // Afficher l'UI
                break;
            case SceneNames.MainMenu:
                Debug.Log("[UIManager] Main Menu loaded.");
                // Afficher l'UI
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
        GameManager.Instance.SetGameState(GameState.Settings);
    }

    public void OnExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
