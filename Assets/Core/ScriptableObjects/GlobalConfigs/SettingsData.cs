[System.Serializable]
public class AudioSettings
{
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float envVolume = 1f;
}

[System.Serializable]
public class VideoSettings
{
    public int resolutionIndex = 0;
    public bool fullscreen = true;
    public int targetFramerate = 60;
    public bool vsync = false;
}

[System.Serializable]
public class GameplaySettings
{
    public bool invertY = false;
    public float sensitivity = 1f;
}

[System.Serializable]
public class InputBindings
{
    public string inputActionOverridesJson;
}

[System.Serializable]
public class SettingsData
{
    public AudioSettings audio = new AudioSettings();
    public VideoSettings video = new VideoSettings();
    public GameplaySettings gameplay = new GameplaySettings();
    public InputBindings inputBindings;

    public SettingsData CopyFrom(SettingsConfig config)
    {
        audio = new AudioSettings
        {
            masterVolume = config.audio.masterVolume,
            musicVolume = config.audio.musicVolume,
            sfxVolume = config.audio.sfxVolume,
            envVolume = config.audio.envVolume,
        };

        video = new VideoSettings
        {
            resolutionIndex = config.video.resolutionIndex,
            fullscreen = config.video.fullscreen,
            targetFramerate = config.video.targetFramerate,
            vsync = config.video.vsync,
        };

        gameplay = new GameplaySettings
        {
            invertY = config.gameplay.invertY,
            sensitivity = config.gameplay.sensitivity,
        };

        inputBindings = new InputBindings();

        return this;
    }
}
