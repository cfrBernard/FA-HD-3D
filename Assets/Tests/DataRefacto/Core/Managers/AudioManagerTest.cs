using UnityEngine;
using UnityEngine.Audio;
using Newtonsoft.Json.Linq;

public class AudioManagerTest : MonoBehaviour
{
    public static AudioManagerTest Instance { get; private set; }

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
        JObject data = SettingsManagerTest.Instance.GetRawSettings();
        JObject audioSettings = data["audio"] as JObject;
        
        ApplyVolumesFromSettings(audioSettings);
    }

    private void ApplyVolumesFromSettings(JObject audioSettings)
    {
        SetMixerVolume("MasterVolume", audioSettings?["masterVolume"]?.Value<float>() ?? 100);
        SetMixerVolume("MusicVolume", audioSettings?["musicVolume"]?.Value<float>() ?? 100);
        SetMixerVolume("SFXVolume", audioSettings?["sfxVolume"]?.Value<float>() ?? 100);
        SetMixerVolume("EnvVolume", audioSettings?["envVolume"]?.Value<float>() ?? 100);
        SetMixerVolume("UIVolume", audioSettings?["uiVolume"]?.Value<float>() ?? 100);
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
}

