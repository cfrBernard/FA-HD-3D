using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("START");
        GameManager.Instance.SetGameState(GameState.Playing);
    }

    public void OpenSettings()
    {
        Debug.Log("SETTINGS");
        GameManager.Instance.SetGameState(GameState.Settings);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
