using UnityEngine;
using System;

public enum GameState
{
    Boot,
    MainMenu,
    Settings,
    Playing,
    Paused,
    Loading
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

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

    public void SetGameState(GameState newState)
    {
        if (newState == CurrentState) return;

        CurrentState = newState;
        Debug.Log($"[GameManager] GameState changed to {newState}");

        switch (newState)
        {
            case GameState.Boot:
                // SceneManager.Instance.LoadScene(SceneNames.Boot);
                break;
            case GameState.MainMenu:
                SceneManager.Instance.LoadScene(SceneNames.MainMenu);
                break;
            case GameState.Playing:
                SceneManager.Instance.LoadSceneWithLoading(SceneNames.Game);
                break;
            case GameState.Settings:
                SceneManager.Instance.LoadAdditiveScene(SceneNames.SettingsMenu);
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}
