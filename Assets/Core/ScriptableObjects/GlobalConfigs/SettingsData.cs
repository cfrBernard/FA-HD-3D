using System;

#region Game
[Serializable]
public class GameplaySettings
{
    public bool invertY = false;
    public float sensitivity = 1f;
}
#endregion

#region Video
public enum ResolutionOption
{
    R1920x1080,
    R2560x1440,
    R3840x2160,
    R1280x720
}

public enum TargetFramerate
{
    FPS30 = 30,
    FPS60 = 60,
    FPS120 = 120,
    FPS144 = 144,
    FPS240 = 240,
    Unlimited = -1
}

[Serializable]
public class VideoSettings
{
    public ResolutionOption resolution = ResolutionOption.R1920x1080;
    public bool fullscreen = true;
    public TargetFramerate targetFramerate = TargetFramerate.FPS60;
    public bool vsync = false;
    public float uiScale = 1f;
    public bool hdr = false;
}
#endregion

# region Graphics
public enum QualityPreset { Low, Medium, High, Ultra }
public enum TextureQuality { Low, Medium, High }
public enum ShadowQuality { Off, Low, High }
public enum AntiAliasing { None, FXAA, TAA }

[Serializable]
public class GraphicsSettings
{
    public QualityPreset qualityPreset = QualityPreset.High;
    public TextureQuality textureQuality = TextureQuality.High;
    public ShadowQuality shadowQuality = ShadowQuality.High;
    public AntiAliasing antiAliasing = AntiAliasing.TAA;

    public bool postProcessing = true;
    public bool ambientOcclusion = true;
    public bool motionBlur = false;
    public bool bloom = true;
    public bool depthOfField = true;
}
#endregion

# region Audio 
[Serializable]
public class AudioSettings
{
    public float masterVolume = 100f;
    public float musicVolume = 100f;
    public float sfxVolume = 100f;
    public float envVolume = 100f;
    public float uiVolume = 100f;
    public bool muteAll = false;
}
#endregion

# region Input
[Serializable]
public class InputBindings
{
    public string inputActionOverridesJson;
}
#endregion

# region Data
[Serializable]
public class SettingsData
{
    public AudioSettings audio = new AudioSettings();
    public VideoSettings video = new VideoSettings();
    public GraphicsSettings graphics = new GraphicsSettings();
    public GameplaySettings gameplay = new GameplaySettings();
    public InputBindings inputBindings;

    public SettingsData CopyFrom(SettingsConfig config)
    {
        gameplay = new GameplaySettings
        {
            invertY = config.gameplay.invertY,
            sensitivity = config.gameplay.sensitivity,
        };

        video = new VideoSettings
        {
            resolution = config.video.resolution,
            fullscreen = config.video.fullscreen,
            targetFramerate = config.video.targetFramerate,
            vsync = config.video.vsync,
            uiScale = config.video.uiScale,
            hdr = config.video.hdr,
        };

        graphics = new GraphicsSettings
        {
            qualityPreset = config.graphics.qualityPreset,
            textureQuality = config.graphics.textureQuality,
            shadowQuality = config.graphics.shadowQuality,
            antiAliasing = config.graphics.antiAliasing,
            postProcessing = config.graphics.postProcessing,
            ambientOcclusion = config.graphics.ambientOcclusion,
            motionBlur = config.graphics.motionBlur,
            bloom = config.graphics.bloom,
            depthOfField = config.graphics.depthOfField,
        };

        audio = new AudioSettings
        {
            masterVolume = config.audio.masterVolume,
            musicVolume = config.audio.musicVolume,
            sfxVolume = config.audio.sfxVolume,
            envVolume = config.audio.envVolume,
            uiVolume = config.audio.uiVolume,
            muteAll = config.audio.muteAll,
        };

        inputBindings = new InputBindings();

        return this;
    }
}
#endregion
