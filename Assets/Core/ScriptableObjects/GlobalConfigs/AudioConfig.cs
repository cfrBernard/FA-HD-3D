using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "GlobalConfigs/Audio Config")]
public class AudioConfig : ScriptableObject
{
    public AudioMixer mainMixer;
}