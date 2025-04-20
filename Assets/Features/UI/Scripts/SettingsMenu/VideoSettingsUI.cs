using UnityEngine;

public class VideoSettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public SettingsCategory categoryAsset;
    public Transform contentPanel;
    public GameObject paramSliderPrefab;
    public GameObject paramTogglePrefab;
    public GameObject paramDropdownPrefab;

    private SettingsData settingsData;

    private void OnEnable() // start ? 
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
                HandleSlider(param);
            }
            else if (param.type == ParamType.Toggle)
            {
                HandleToggle(param);
            }
            else if (param.type == ParamType.Dropdown)
            {
                HandleDropdown(param);
            }
        }
    }

    private void HandleSlider(ParamDefinition param)
    {
        float currentValue = (float)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);
        CreateSlider(param, currentValue);
    }

    private void CreateSlider(ParamDefinition paramDef, float currentValue)
    {
        var go = Instantiate(paramSliderPrefab, contentPanel);
        var slider = go.GetComponent<ParamSlider>();

        slider.Setup(
            paramDef.label,
            currentValue,
            value =>
            {
                ReflectionUtils.SetValueByPath(settingsData, paramDef.key, value);
                ApplyAndSave();
            },
            paramDef.showDecimal,
            paramDef.minValue,
            paramDef.maxValue
        );
    }

    private void HandleToggle(ParamDefinition param)
    {
        bool currentValue = (bool)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);
        CreateGeneralToggle(param, currentValue);
    }

    private void CreateGeneralToggle(ParamDefinition param, bool currentValue)
    {
        var go = Instantiate(paramTogglePrefab, contentPanel);
        var toggle = go.GetComponent<ParamToggle>();
        toggle.Setup(param.label, currentValue, value =>
        {
            ReflectionUtils.SetValueByPath(settingsData, param.propertyPath, value);
            ApplyAndSave();
        });
    }

    private void HandleDropdown(ParamDefinition param)
    {
        int currentValue = (int)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);
        CreateDropdown(param, currentValue);
    }

    private void CreateDropdown(ParamDefinition param, int currentValue)
    {
        var go = Instantiate(paramDropdownPrefab, contentPanel);
        var dropdown = go.GetComponent<ParamDropdown>();

        dropdown.Setup(param.label, param.dropdownOptions, currentValue, value =>
        {
            ReflectionUtils.SetValueByPath(settingsData, param.key, value);
            ApplyAndSave();
        });
    }

    private void ApplyAndSave()
    {
        SettingsManager.Instance.Save();
        SettingsManager.Instance.ApplySettings();
    } 
}
