using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/Audio Config")]
public class AudioConfig : ScriptableObject
{
    public AudioMixer mainMixer;
}