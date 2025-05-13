using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private static string UserSettingsPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            UserSettingsPath = Path.Combine(Application.persistentDataPath, "UserSettings.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SaveSettings(JObject userData)
    {
        string json = userData.ToString();
        File.WriteAllText(UserSettingsPath, json);
        Debug.Log($"[SaveManager] Saved user settings to {UserSettingsPath}");
    }

    public static JObject LoadSettings()
    {
        if (!File.Exists(UserSettingsPath))
        {
            Debug.LogWarning("[SaveManager] No user settings found.");
            return new JObject();
        }

        string json = File.ReadAllText(UserSettingsPath);
        return JObject.Parse(json);
    }
}
