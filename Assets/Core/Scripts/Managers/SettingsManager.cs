using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private JObject defaultSettings;
    private JObject userSettings;
    private JObject metadataSettings;
    
    private const string DefaultSettingsPath = "DefaultSettings";
    private const string MetadataSettingsPath = "MetadataSettings";

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

        // Load metadata from Resources
        TextAsset metadataAsset = Resources.Load<TextAsset>(MetadataSettingsPath);
        if (metadataAsset == null)
        {
            Debug.LogError("[SettingsManager] MetadataSettings.json not found in Resources/");
            return;
        }
        metadataSettings = JObject.Parse(metadataAsset.text);

        // Load user overrides if exists
        if (File.Exists(GetUserSettingsPath()))
        {
            string userJson = File.ReadAllText(GetUserSettingsPath());
            userSettings = JObject.Parse(userJson);
        }
        else // write empty override
        {
            userSettings = new JObject();
            Save();
        }

        ApplySettings(); // too early? (fix: "X"Manager.start())
    }

    public void ApplySettings()
    {
        // TESTING v2
        AudioManager.Instance?.UpdateVolumes();
        VideoManager.Instance?.ApplyVideoSettings();
        VideoManager.Instance?.ApplyGraphicsSettings();
    }

    public void Save()
    {
        SaveManager.SaveSettings(userSettings);
    }

    public void ResetAllSettings() // TODO : Need UI Update too
    {
        if (File.Exists(GetUserSettingsPath()))
        {
            File.Delete(GetUserSettingsPath());
        }

        InputManager.Instance.ResetAllBindings();

        userSettings = new JObject();
        Save();
        ApplySettings();
    }

    public void ResetSettings()
    {
        // TODO : Reset by path
    }

    #region Get/Set
    public JObject GetDefaultSettings()
    {
        return defaultSettings;
    }

    public JObject GetMetadataSettings()
    {
        return metadataSettings;
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
    #endregion
}

