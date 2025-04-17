using UnityEngine;

[CreateAssetMenu(fileName = "SettingsConfig", menuName = "GlobalConfigs/SettingsConfig")]
public class SettingsConfig : ScriptableObject
{
    [Header("Gameplay")]
    public GameplaySettings gameplay = new GameplaySettings();

    [Header("Video")]
    public VideoSettings video = new VideoSettings();
    
    [Header("Graphics")]
    public GraphicsSettings graphics = new GraphicsSettings();

    [Header("Audio")]
    public AudioSettings audio = new AudioSettings();
}
