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
        // Appliquer les paramètres de volume depuis SettingsData
        audioMixer.SetFloat("MasterVolume", data.masterVolume);
        audioMixer.SetFloat("MusicVolume", data.musicVolume);
        audioMixer.SetFloat("SFXVolume", data.sfxVolume);
        audioMixer.SetFloat("EnvVolume", data.envVolume);
    }
}
