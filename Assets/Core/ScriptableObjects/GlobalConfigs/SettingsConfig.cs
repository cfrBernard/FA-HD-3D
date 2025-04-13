using UnityEngine;

[CreateAssetMenu(fileName = "SettingsConfig", menuName = "GlobalConfigs/SettingsConfig")]
public class SettingsConfig : ScriptableObject
{
    [Header("Audio")]
    public float masterVolume = 1f;
    public float musicVolume = 0.7f;
    public float sfxVolume = 1f;

    [Header("Video")]
    public int resolutionIndex = 0;  // index dans une liste Unity
    public bool fullscreen = true;
    public int targetFramerate = 60;
    public bool vsync = false;

    [Header("Gameplay")]
    public bool invertY = false;
    public float sensitivity = 1.0f;

    [Header("Input")]
    public InputBindings defaultInputBindings;
}
