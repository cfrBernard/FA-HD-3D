using UnityEngine;

public class SettingsResetter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SettingsManager.Instance.ResetAllSettings();
        }
    }
}
