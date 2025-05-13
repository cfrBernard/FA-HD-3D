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
        UpdateVolumes();
    }

    public void UpdateVolumes()
    {
        ApplyVolumesFromSettings();
    }

    private void ApplyVolumesFromSettings()
    {
        SetMixerVolume("MasterVolume", GetAudioSetting("masterVolume"));
        SetMixerVolume("MusicVolume", GetAudioSetting("musicVolume"));
        SetMixerVolume("SFXVolume", GetAudioSetting("sfxVolume"));
        SetMixerVolume("EnvVolume", GetAudioSetting("envVolume"));
        SetMixerVolume("UIVolume", GetAudioSetting("uiVolume"));
    }

    private void SetMixerVolume(string exposedParam, float sliderValue)
    {
        float db = ConvertToDB(sliderValue);
        audioMixer.SetFloat(exposedParam, db);
    }

    private float ConvertToDB(float sliderValue)
    {
        if (sliderValue <= 0f) return -80f;
        float normalized = sliderValue / 100f;
        return Mathf.Lerp(-80f, 0f, normalized);
    }

    private float GetAudioSetting(string field, float defaultValue = 100f)
    {
        try
        {
            return SettingsManager.Instance.GetSetting<float>("audio", field);
        }
        catch
        {
            Debug.LogWarning($"[AudioManager] Audio setting '{field}' missing, using default value : {defaultValue}");
            return defaultValue;
        }
    }
}

