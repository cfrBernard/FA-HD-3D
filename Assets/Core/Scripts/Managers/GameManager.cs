using UnityEngine;

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

    void Awake()
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
        CurrentState = newState;
        EventManager.TriggerEvent("OnGameStateChanged", newState);
    }
}
