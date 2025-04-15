using UnityEngine;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public SettingsCategory categoryAsset;
    public Transform contentPanel;
    public GameObject paramSliderPrefab;

    private SettingsData settingsData;

    private void OnEnable()
    {
        settingsData = SettingsManager.Instance.GetSettingsData();
        GenerateUI();
    }

    private void GenerateUI()
    {
        foreach (var param in categoryAsset.parameters)
        {
            if (param.type == ParamType.Slider)
            {
                float currentValue = (float)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);


                CreateSlider(param, currentValue);
            }
            // Toggle, Dropdown
        }
    }

    private void CreateSlider(ParamDefinition paramDef, float currentValue)
    {
        var go = Instantiate(paramSliderPrefab, contentPanel);
        var slider = go.GetComponent<ParamSlider>();

        slider.Setup(paramDef.label, currentValue, value =>
        {
            ReflectionUtils.SetValueByPath(settingsData, paramDef.key, value);
            ApplyAndSave();
        }, paramDef.showDecimal);
    }

    private void ApplyAndSave()
    {
        SettingsManager.Instance.Save();
        SettingsManager.Instance.ApplySettings();
    }
}
