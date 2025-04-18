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
        SettingsData data = SettingsManager.Instance.GetSettingsData();
        ApplyVideoSettings(data);
        ApplyGraphicsSettings(data);
    }

    public void ApplyVideoSettings(SettingsData data)
    {
        VideoSettings video = data.video;

        // Resolution / fullscreen
        ApplyResolution(video.resolution, video.fullscreen);

        // VSync
        QualitySettings.vSyncCount = video.vsync ? 1 : 0;
        Debug.Log("VSync count set to: " + QualitySettings.vSyncCount);

        // Framerate cap
        TargetFramerate targetFramerate = TargetFramerateFromIndex((int)video.targetFramerate);
        ApplyTargetFramerate(targetFramerate);
    }

    public void ApplyGraphicsSettings(SettingsData data) // let him sleep
    {
        GraphicsSettings graphics = data.graphics;

        // Quality Preset
        QualitySettings.SetQualityLevel((int)graphics.qualityPreset);

        // Texture Quality
        // QualitySettings.globalTextureMipmapLimit: 0 = full res, 1 = half, etc.
        // QualitySettings.globalTextureMipmapLimit = 2 - (int)graphics.textureQuality;

        // Shadow Quality
        // TODO: Map shadow setting to Unity shadow options
        // ApplyShadowQuality(graphics.shadowQuality);

        // AntiAliasing
        // TODO: Switch render pipeline AA or custom implementation
        // ApplyAntiAliasing(graphics.antiAliasing);

        // Post FX
        // TODO: These require Post Processing Volume or URP/HDRP toggle logic
        // PostProcessingManager.Instance.SetEffect("AO", graphics.ambientOcclusion);
        // PostProcessingManager.Instance.SetEffect("Bloom", graphics.bloom);
        // etc.
    }

    private void ApplyResolution(ResolutionOption option, bool fullscreen)
    {
        Resolution res = ResolutionFromOption(option);
        Screen.SetResolution(res.width, res.height, fullscreen);
    }

    private Resolution ResolutionFromOption(ResolutionOption option)
    {
        return option switch
        {
            ResolutionOption.R1920x1080 => new Resolution { width = 1920, height = 1080 },
            ResolutionOption.R2560x1440 => new Resolution { width = 2560, height = 1440 },
            ResolutionOption.R3840x2160 => new Resolution { width = 3840, height = 2160 },
            ResolutionOption.R1280x720  => new Resolution { width = 1280, height = 720 },
            _ => Screen.currentResolution
        };
    }

    private void ApplyTargetFramerate(TargetFramerate target)
    {
        int fpsValue = (int)target;

        if (QualitySettings.vSyncCount > 0)
        {
            Application.targetFrameRate = -1;
            Debug.Log("VSync is active, targetFrameRate ignored");
        }
        else
        {
            Application.targetFrameRate = fpsValue;
            Debug.Log("Frame cap set to: " + fpsValue);
        }
    }

    private TargetFramerate TargetFramerateFromIndex(int index)
    {
        switch (index)
        {
            case 0: return TargetFramerate.FPS30;
            case 1: return TargetFramerate.FPS60;
            case 2: return TargetFramerate.FPS120;
            case 3: return TargetFramerate.FPS144;
            case 4: return TargetFramerate.FPS240;
            case 5: return TargetFramerate.Unlimited;
            default: return TargetFramerate.FPS60;
        }
    }

    private void ApplyShadowQuality(ShadowQuality quality)
    {
        switch (quality)
        {
            case ShadowQuality.Off:
                QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
                break;
            case ShadowQuality.Low:
                QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
                break;
            case ShadowQuality.High:
                QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                break;
        }
    }

    private void ApplyAntiAliasing(AntiAliasing aa)
    {
        // Placeholder: this depends on your pipeline
        // Example:
        // URPRenderer.SetAntiAliasing(aa);
        Debug.Log($"AntiAliasing set to: {aa}");
    }
}
