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
            Debug.LogError("[SettingsManagerTest] DefaultSettings.json not found in Resources/");
            return;
        }
        defaultSettings = JObject.Parse(defaultAsset.text);

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
        AudioManagerTest.Instance?.UpdateVolumes();
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

    # region Get/Set
    public JObject GetDefaultSettings()
    {
        return defaultSettings;
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

        Debug.LogWarning($"[SettingsManagerTest] Setting not found: {path}.{field}");
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
    
        // Créer une structure { field: value } si c'est un simple champ
        if (current[field] == null || current[field].Type != JTokenType.Object)
        {
            current[field] = new JObject
            {
                ["default"] = JToken.FromObject(value)
            };
        }
        else
        {
            current[field]["default"] = JToken.FromObject(value);  // par défaut
        }
    }
    # endregion
}
