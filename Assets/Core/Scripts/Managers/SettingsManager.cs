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
            // DebugTestValues();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void DebugTestValues() // TEST
    {
        Debug.Log("=== SETTINGS TEST START ===");

        // 1. Shows basic values
        Debug.Log($"[TEST] Before: MasterVolume (data) = {_data.audio.masterVolume}");
        Debug.Log($"[TEST] Before: MasterVolume (config) = {GlobalConfigs.Settings.audio.masterVolume}");

        // 2. Change value and save
        _data.audio.masterVolume = 0.321f;
        SaveManager.SaveSettings(_data);
        Debug.Log($"[TEST] Modified and saved MasterVolume = {_data.audio.masterVolume}");

        // 3. Load from disk
        var loadedData = SaveManager.LoadSettings();

        Debug.Log($"[TEST] After reload: MasterVolume (loadedData) = {loadedData.audio.masterVolume}");
        Debug.Log($"[TEST] After reload: MasterVolume (config should still be untouched) = {GlobalConfigs.Settings.audio.masterVolume}");

        Debug.Log("=== SETTINGS TEST END ===");
    }

    private void LoadOrInitSettings()
    {
        var saved = SaveManager.LoadSettings();
        _data = saved ?? new SettingsData().CopyFrom(GlobalConfigs.Settings);

        ApplySettings(); // too early? (fix: AudioManager(start))
    }

    public void ApplySettings()
    {
        AudioManager.Instance?.UpdateVolumes(_data);
        VideoManager.Instance?.ApplyVideoSettings(_data);
        VideoManager.Instance?.ApplyGraphicsSettings(_data);
        InputManager.Instance?.UpdateBindings(_data);
    }

    public void Save()
    {
        Debug.Log("[SettingsManager] Save() called");
        SaveManager.SaveSettings(_data);
    }

    public void ResetToDefault()
    {
        _data.CopyFrom(GlobalConfigs.Settings);
        ApplySettings();
        Save();
    }

    public SettingsData GetSettingsData() => _data;
}
