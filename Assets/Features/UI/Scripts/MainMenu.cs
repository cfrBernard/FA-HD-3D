using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("START");
        SceneManager.Instance.LoadScene("GameplayScene");
    }

    public void OpenSettings()
    {
        Debug.Log("SETTINGS");
        SceneManager.Instance.LoadAdditiveScene("SettingsMenu");
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }
}
