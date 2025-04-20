using System;

#region Game
[Serializable]
public enum CrosshairStyle
{
    Default,
    Dot,
    Circle,
    Cross,
    None
}

public enum SubtitleSize
{
    Small,
    Medium,
    Large
}

[Serializable]
public class GameplaySettings
{
    // range 60 - 120
    public float fov = 90f;
    
    // public bool subtitles = true;
    // public SubtitleSize subtitleSize = SubtitleSize.Medium;
    // public bool autoSave = true;
    // public bool cameraShake = true;
    // public bool headbob = true;
    // public CrosshairStyle crosshairStyle = CrosshairStyle.Default;
    // public float crosshairSize = 1f;
    
    // ColorUtility for color picker ???
    // public string crosshairColorHex = "#FFFFFF";
}
#endregion

#region Video
public enum ResolutionOption
{
    R1280x720,
    R1920x1080,
    R2560x1440,
    R3840x2160
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
    
    // public float uiScale = 1f;
    // public bool hdr = false;
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

# region Controls
[Serializable]
public class KBMSettings
{
    // range 0.01 - 5 
    public float mouseSensitivityX = 1f;
    
    // range 0.01 - 5 
    public float mouseSensitivityY = 1f;
    public bool invertMouseX = false;
    public bool invertMouseY = false;
    public bool mouseSmoothing = false;

    // range 0.01 - 5 
    public float smoothingAmount = 0.5f;
}

[Serializable]
public class GamepadSettings
{
    // range 0.01 - 5 
    public float stickSensitivityX = 1f;
    
    // range 0.01 - 5 
    public float stickSensitivityY = 1f;
    public bool invertStickX = false;
    public bool invertStickY = false;
    
    // range 0.01 - 1 
    public float deadzoneLeft = 0.1f;
    
    // range 0.01 - 1 
    public float deadzoneRight = 0.1f;
    
    // public bool aimAssist = true;
}

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
    public GameplaySettings gameplay = new GameplaySettings();
    public VideoSettings video = new VideoSettings();
    public GraphicsSettings graphics = new GraphicsSettings();
    public AudioSettings audio = new AudioSettings();
    public KBMSettings kbm = new KBMSettings();
    public GamepadSettings gamepad = new GamepadSettings();
    
    public InputBindings inputBindings;

    public SettingsData CopyFrom(SettingsConfig config)
    {
        gameplay = new GameplaySettings
        {
            fov = config.gameplay.fov,
            
            // subtitles = config.gameplay.subtitles,
            // subtitleSize = config.gameplay.subtitleSize,
            // autoSave = config.gameplay.autoSave,
            // cameraShake = config.gameplay.cameraShake,
            // headbob = config.gameplay.headbob,
            // crosshairStyle = config.gameplay.crosshairStyle,
            // crosshairSize = config.gameplay.crosshairSize,
            // crosshairColorHex = config.gameplay.crosshairColorHex,
        };

        video = new VideoSettings
        {
            resolution = config.video.resolution,
            fullscreen = config.video.fullscreen,
            targetFramerate = config.video.targetFramerate,
            vsync = config.video.vsync,
            
            // uiScale = config.video.uiScale,
            // hdr = config.video.hdr,
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

        kbm = new KBMSettings
        {
            mouseSensitivityX = config.kbm.mouseSensitivityX,
            mouseSensitivityY = config.kbm.mouseSensitivityY,
            invertMouseX = config.kbm.invertMouseX,
            invertMouseY = config.kbm.invertMouseY,
            mouseSmoothing = config.kbm.mouseSmoothing,
            smoothingAmount = config.kbm.smoothingAmount
        };

        gamepad = new GamepadSettings
        {
            stickSensitivityX = config.gamepad.stickSensitivityX,
            stickSensitivityY = config.gamepad.stickSensitivityY,
            invertStickX = config.gamepad.invertStickX,
            invertStickY = config.gamepad.invertStickY,
            deadzoneLeft = config.gamepad.deadzoneLeft,
            deadzoneRight = config.gamepad.deadzoneRight,
            
            // aimAssist = config.gamepad.aimAssist
        };


        inputBindings = new InputBindings();

        return this;
    }
}
#endregion
