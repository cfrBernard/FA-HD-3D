using UnityEngine;

[CreateAssetMenu(fileName = "SettingsConfig", menuName = "GlobalConfigs/SettingsConfig")]
public class SettingsConfig : ScriptableObject
{
    [Header("Audio")]
    public AudioSettings audio = new AudioSettings();

    [Header("Video")]
    public VideoSettings video = new VideoSettings();

    [Header("Gameplay")]
    public GameplaySettings gameplay = new GameplaySettings();
}
