using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SettingsManagerTest : MonoBehaviour
{
    public static SettingsManagerTest Instance { get; private set; }

    private JObject defaultSettings;
    private JObject userSettings;

    
    // Paths
    private const string DefaultSettingsPath = "DefaultSettings";
    private string GetUserSettingsPath()
    {
        return Path.Combine(Application.persistentDataPath, "UserSettings.json");
    }
    

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

    public void LoadOrInitSettings()
    {
        // Load defaults from Resources
        TextAsset defaultAsset = Resources.Load<TextAsset>(DefaultSettingsPath);
        if (defaultAsset == null)
        {
            Debug.LogError("[SettingsManager] DefaultSettings.json not found in Resources/");
            return;
        }
        defaultSettings = JObject.Parse(defaultAsset.text);

        // Load user overrides if exists
        if (File.Exists(GetUserSettingsPath()))
        {
            string userJson = File.ReadAllText(GetUserSettingsPath());
            userSettings = JObject.Parse(userJson);
        }
        else
        {
            userSettings = new JObject();
            Save(); // write empty override file
        }

        ApplySettings();
    }

    public void ApplySettings()
    {
        // TESTING
        float fov = GetSetting<float>("gameplay.fov", "default");
        Debug.Log($"[SettingsManager] Applied FOV: {fov}");

        // AudioManager.Instance?.UpdateVolumes(data??);
        // VideoManager.Instance?.ApplyVideoSettings(data??);
        // VideoManager.Instance?.ApplyGraphicsSettings(data??);
        // InputManager.Instance?.UpdateBindings(data??);
    }

    public void Save()
    {
        SaveManagerTest.SaveSettings(userSettings);
    }

    public void ResetSettings()
    {
        if (File.Exists(GetUserSettingsPath()))
        {
            File.Delete(GetUserSettingsPath());
        }

        userSettings = new JObject();
        Save();
        ApplySettings();
    }

    public JObject GetRawSettings()
    {
        return userSettings;
    }

    public T GetSetting<T>(string path, string field)
    {
        // Check user override first
        var userToken = userSettings.SelectToken($"{path}.{field}");
        if (userToken != null)
            return userToken.Value<T>();

        // Fall back to default
        var defaultToken = defaultSettings.SelectToken($"{path}.{field}");
        if (defaultToken != null)
            return defaultToken.Value<T>();

        Debug.LogWarning($"[SettingsManager] Setting not found: {path}.{field}");
        return default;
    }

    public void SetOverride<T>(string path, string field, T value)
    {
        var sections = path.Split('.');
        JObject current = userSettings;

        foreach (var section in sections)
        {
            if (current[section] == null)
                current[section] = new JObject();

            current = (JObject)current[section];
        }

        current[field] = JToken.FromObject(value);
    }

    // TESTING
    #if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(10, 10, 300, 130), "Settings Debug", GUI.skin.window);

        if (GUI.Button(new Rect(10, 20, 280, 30), "Reset Settings"))
        {
            ResetSettings();
        }

        if (GUI.Button(new Rect(10, 60, 280, 30), "Print Current FOV"))
        {
            float fov = GetSetting<float>("gameplay.fov", "default");
            Debug.Log("[SettingsManager][DebugGUI] Current FOV: " + fov);
        }

        if (GUI.Button(new Rect(10, 100, 280, 30), "Override FOV to 110"))
        {
            SetOverride("gameplay.fov", "default", 110f);
            Save();
            Debug.Log("[SettingsManager][DebugGUI] FOV overridden and saved.");
        }

        GUI.EndGroup();
    }
    #endif

}
