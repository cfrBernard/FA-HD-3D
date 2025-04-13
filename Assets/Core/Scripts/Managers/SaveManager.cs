using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour 
{
    public static SaveManager Instance { get; private set; }
    private static string SavePath => Path.Combine(Application.persistentDataPath, "settings.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SaveSettings(SettingsData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveManager] Settings saved to {SavePath}");
    }

    public static SettingsData LoadSettings()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveManager] No save file found, using default settings.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SettingsData>(json);
    }
}
