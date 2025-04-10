using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("START");
        // SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        // 
        Debug.Log("SETTINGS");
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
