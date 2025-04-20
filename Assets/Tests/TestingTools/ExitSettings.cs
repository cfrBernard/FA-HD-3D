using UnityEngine;

public class ExitSettings : MonoBehaviour
{
    public void OnExitSettings()
    {
        SceneManager.Instance.UnloadScene(SceneNames.SettingsMenu);
        Debug.Log("EXIT");
    }
}
