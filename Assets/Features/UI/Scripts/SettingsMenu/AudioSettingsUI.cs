using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider envVolumeSlider;

    private void OnEnable()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settingsData = SettingsManager.Instance.GetSettingsData();

        // loading values
        masterVolumeSlider.value = settingsData.masterVolume;
        musicVolumeSlider.value = settingsData.musicVolume;
        sfxVolumeSlider.value = settingsData.sfxVolume;
        envVolumeSlider.value = settingsData.envVolume;

        // onValueChanged
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        envVolumeSlider.onValueChanged.AddListener(OnEnvVolumeChanged);
    }

    public void OnMasterVolumeChanged(float value)
    {
        var settingsData = SettingsManager.Instance.GetSettingsData();
        settingsData.masterVolume = value;
        SettingsManager.Instance.Save();
        SettingsManager.Instance.ApplySettings();
    }

    public void OnMusicVolumeChanged(float value)
    {
        var settingsData = SettingsManager.Instance.GetSettingsData();
        settingsData.musicVolume = value; 
        SettingsManager.Instance.Save(); 
        SettingsManager.Instance.ApplySettings(); 
    }

    public void OnSFXVolumeChanged(float value)
    {
        var settingsData = SettingsManager.Instance.GetSettingsData();
        settingsData.sfxVolume = value;  
        SettingsManager.Instance.Save(); 
        SettingsManager.Instance.ApplySettings(); 
    }

    public void OnEnvVolumeChanged(float value)
    {
        var settingsData = SettingsManager.Instance.GetSettingsData();
        settingsData.envVolume = value;  
        SettingsManager.Instance.Save(); 
        SettingsManager.Instance.ApplySettings(); 
    }
}
