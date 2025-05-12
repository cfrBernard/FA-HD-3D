using UnityEngine;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance { get; private set; }

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

    private void Start()
    {
        ApplyVideoSettings(); // Abstract
        ApplyGraphicsSettings(); // Abstract 
    }

    public void ApplyVideoSettings()
    {
        string resolution = SettingsManagerTest.Instance.GetSetting<string>("video", "resolution");
        bool fullscreen = SettingsManagerTest.Instance.GetSetting<bool>("video", "fullscreen");
        string framerateStr = SettingsManagerTest.Instance.GetSetting<string>("video", "targetFramerate");
        bool vsync = SettingsManagerTest.Instance.GetSetting<bool>("video", "vsync");

        // Résolution
        string[] resParts = resolution.Split('x');
        if (resParts.Length == 2 &&
            int.TryParse(resParts[0], out int width) &&
            int.TryParse(resParts[1], out int height))
        {
            Screen.SetResolution(width, height, fullscreen);
        }
        else
        {
            Debug.LogWarning("Résolution invalide : " + resolution);
        }

        // VSync
        QualitySettings.vSyncCount = vsync ? 1 : 0;

        // Framerate
        if (int.TryParse(framerateStr.Replace("FPS", ""), out int targetFPS))
        {
            Application.targetFrameRate = targetFPS;
        }
        else
        {
            Debug.LogWarning("Framerate invalide : " + framerateStr);
        }
    }

    public void ApplyGraphicsSettings()
    {
        string qualityPreset = SettingsManagerTest.Instance.GetSetting<string>("graphics", "qualityPreset");
        string textureQuality = SettingsManagerTest.Instance.GetSetting<string>("graphics", "textureQuality");
        string shadowQuality = SettingsManagerTest.Instance.GetSetting<string>("graphics", "shadowQuality");
        string antiAliasing = SettingsManagerTest.Instance.GetSetting<string>("graphics", "antiAliasing");

        bool postProcessing = SettingsManagerTest.Instance.GetSetting<bool>("graphics", "postProcessing");
        bool ambientOcclusion = SettingsManagerTest.Instance.GetSetting<bool>("graphics", "ambientOcclusion");
        bool motionBlur = SettingsManagerTest.Instance.GetSetting<bool>("graphics", "motionBlur");
        bool bloom = SettingsManagerTest.Instance.GetSetting<bool>("graphics", "bloom");
        bool depthOfField = SettingsManagerTest.Instance.GetSetting<bool>("graphics", "depthOfField");

        // Qualité globale
        QualitySettings.SetQualityLevel(QualityPresetToIndex(qualityPreset), true);

        // Anti-Aliasing
        // QualitySettings.antiAliasing = AntiAliasingToSampleCount(antiAliasing);
        Debug.Log("antiAliasing: " + antiAliasing);

        // Shadow & Texture quality
        Debug.Log("Texture Quality: " + textureQuality);
        Debug.Log("Shadow Quality: " + shadowQuality);

        // These require Post Processing Volume or URP/HDRP toggle logic
        Debug.Log($"PostFX - Bloom: {bloom}, DOF: {depthOfField}, AO: {ambientOcclusion}, MotionBlur: {motionBlur}, PostProcessing: {postProcessing}");
    }

    private int QualityPresetToIndex(string preset)
    {
        switch (preset.ToLower())
        {
            case "low": return 0;
            case "medium": return 1;
            case "high": return 2;
            case "ultra": return 3;
            default: return 2; // default to High
        }
    }

    private int AntiAliasingToSampleCount(string aa)
    {
        switch (aa.ToUpper())
        {
            case "NONE": return 0;
            case "FXAA": return 0; // handled via post-processing
            case "TAA": return 0;  // handled via post-processing
            case "2X": return 2;
            case "4X": return 4;
            case "8X": return 8;
            default: return 0;
        }
    }
}
