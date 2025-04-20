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

    private void Start()
    {
        SettingsData data = SettingsManager.Instance.GetSettingsData();
        UpdateVolumes(data);
    }

    public void UpdateVolumes(SettingsData data)
    {
        if (data.audio.muteAll)
        {
            SetVolumeToMute();
        }
        else
        {
            SetVolumeFromSliderValues(data);
        }
    }

    private void SetVolumeToMute()
    {
        audioMixer.SetFloat("MasterVolume", ConvertToDB(0));
        audioMixer.SetFloat("MusicVolume", ConvertToDB(0)); 
        audioMixer.SetFloat("SFXVolume", ConvertToDB(0)); 
        audioMixer.SetFloat("EnvVolume", ConvertToDB(0)); 
        audioMixer.SetFloat("UIVolume", ConvertToDB(0)); 
    }

    private void SetVolumeFromSliderValues(SettingsData data)
    {
        audioMixer.SetFloat("MasterVolume", ConvertToDB(data.audio.masterVolume));
        audioMixer.SetFloat("MusicVolume", ConvertToDB(data.audio.musicVolume));
        audioMixer.SetFloat("SFXVolume", ConvertToDB(data.audio.sfxVolume));
        audioMixer.SetFloat("EnvVolume", ConvertToDB(data.audio.envVolume));
        audioMixer.SetFloat("UIVolume", ConvertToDB(data.audio.uiVolume));
    }

    private float ConvertToDB(float sliderValue)
    {
        if (sliderValue <= 0) return -80f;

        // Normalise 0-100 en ratio entre 0 et 1
        float normalizedValue = sliderValue / 100f;

        // Interpolation linéaire entre -80 dB et 0 dB
        return Mathf.Lerp(-80f, 0f, normalizedValue);
    }
}
