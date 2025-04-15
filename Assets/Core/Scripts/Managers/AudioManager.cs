using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioMixer audioMixer;

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

        audioMixer = GlobalConfigs.Audio.mainMixer;
    }

    public void UpdateVolumes(SettingsData data)
    {
        // Appliquer depuis SettingsData
        audioMixer.SetFloat("MasterVolume", data.audio.masterVolume);
        audioMixer.SetFloat("MusicVolume", data.audio.musicVolume);
        audioMixer.SetFloat("SFXVolume", data.audio.sfxVolume);
        audioMixer.SetFloat("EnvVolume", data.audio.envVolume);
    }
}
