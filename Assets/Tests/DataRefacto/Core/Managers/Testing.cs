using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Start()
    {
        SettingsManagerTest.Instance.LoadOrInitSettings();

        // DATA TESTING
        SettingsManagerTest.Instance.SetOverride("gameplay.fov", "default", 100f);
        SettingsManagerTest.Instance.Save();
    }
}
