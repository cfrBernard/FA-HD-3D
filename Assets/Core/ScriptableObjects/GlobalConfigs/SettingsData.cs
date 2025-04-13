[System.Serializable]
public class SettingsData
{
    // Audio
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    // Video
    public int resolutionIndex;
    public bool fullscreen;
    public int targetFramerate;
    public bool vsync;

    // Gameplay
    public bool invertY;
    public float sensitivity;

    // Input
    public InputBindings inputBindings;

    public SettingsData CopyFrom(SettingsConfig config)
    {
        masterVolume = config.masterVolume;
        musicVolume = config.musicVolume;
        sfxVolume = config.sfxVolume;

        resolutionIndex = config.resolutionIndex;
        fullscreen = config.fullscreen;
        targetFramerate = config.targetFramerate;
        vsync = config.vsync;

        invertY = config.invertY;
        sensitivity = config.sensitivity;

        inputBindings = new InputBindings
        {
            inputActionOverridesJson = config.defaultInputBindings.inputActionOverridesJson
        };

        return this;
    }
}
