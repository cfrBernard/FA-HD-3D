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

        // Resolution
        // TODO: Map enum to actual resolution and apply via Screen.SetResolution
        // Example: Screen.SetResolution(1920, 1080, settings.fullscreen);
        // ApplyResolution(video.resolution, video.fullscreen);

        // VSync
        //QualitySettings.vSyncCount = video.vsync ? 1 : 0;

        // Framerate cap
        // TODO: If TargetFramerate.Unlimited, set to -1, otherwise set Application.targetFrameRate
        // ApplyTargetFramerate(video.targetFramerate);

        // UI Scale
        // TODO: Send event or notify UI scaler system
        // UIManager.Instance.SetUIScale(video.uiScale);

        // HDR
        // TODO: Needs to be handled per camera or render pipeline
        // GraphicsSettingsManager.Instance.SetHDR(video.hdr);
    }

    public void ApplyGraphicsSettings(SettingsData data)
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
        Application.targetFrameRate = (int)target;
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
