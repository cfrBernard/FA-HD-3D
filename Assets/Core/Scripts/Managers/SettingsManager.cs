using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private SettingsData _data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadOrInitSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadOrInitSettings()
    {
        var saved = SaveManager.LoadSettings();
        _data = saved ?? new SettingsData().CopyFrom(GlobalConfigs.Settings);

        ApplySettings();
    }

    public void ApplySettings()
    {
        // AudioManager.Instance?.UpdateVolumes(_data);
        // InputManager.Instance?.Initialize(_data.inputBindings);
    }

    public void Save()
    {
        SaveManager.SaveSettings(_data);
    }

    public SettingsData GetSettingsData() => _data;

    public void ResetToDefault()
    {
        _data.CopyFrom(GlobalConfigs.Settings);
        ApplySettings();
        Save();
    }
}
