using UnityEngine;
using Newtonsoft.Json.Linq;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform contentPanel;
    public GameObject paramSliderPrefab;
    public GameObject paramTogglePrefab;
    public GameObject paramDropdownPrefab;

    private void OnEnable()
    {
        GenerateUI();
    }

    private void GenerateUI()
    {
        JObject metadata = Resources.Load<TextAsset>("MetadataSettings") is TextAsset metaAsset
            ? JObject.Parse(metaAsset.text)?["audio"] as JObject
            : null;

        if (metadata == null)
        {
            Debug.LogError("[AudioSettingsUI] Metadata not found or invalid for audio.");
            return;
        }

        foreach (var pair in metadata)
        {
            string key = pair.Key;
            JObject param = pair.Value as JObject;
            if (param == null) continue;

            string type = param["type"]?.ToString();
            string label = param["label"]?.ToString() ?? key;

            switch (type)
            {
                case "slider":
                    float sliderValue = SettingsManager.Instance.GetSetting<float>("audio", key);
                    CreateSlider(param, key, sliderValue);
                    break;

                case "toggle":
                    bool toggleValue = SettingsManager.Instance.GetSetting<bool>("audio", key);
                    CreateToggle(param, key, toggleValue);
                    break;

                case "dropdown":
                    string dropdownValue = SettingsManager.Instance.GetSetting<string>("audio", key);
                    CreateDropdown(param, key, dropdownValue);
                    break;

                default:
                    Debug.LogWarning($"[AudioSettingsUI] Unknown type '{type}' for setting '{key}'.");
                    break;
            }
        }
    }


    private void CreateSlider(JObject param, string key, float currentValue)
    {
        var go = Instantiate(paramSliderPrefab, contentPanel);
        var slider = go.GetComponent<ParamSlider>();
        slider.Setup(
            param["label"]?.ToString(),
            currentValue,
            value =>
            {
                SettingsManager.Instance.SetOverride("audio", key, value);
                ApplyAndSave();
            },
            param["decimal"]?.Value<bool>() ?? false,
            param["min"]?.Value<float>() ?? 0,
            param["max"]?.Value<float>() ?? 100
        );
    }

    private void CreateToggle(JObject param, string key, bool currentValue)
    {
        var go = Instantiate(paramTogglePrefab, contentPanel);
        var toggle = go.GetComponent<ParamToggle>();
        toggle.Setup(param["label"]?.ToString(), currentValue, value =>
        {
            SettingsManager.Instance.SetOverride("audio", key, value);
            ApplyAndSave();
        });
    }

    private void CreateDropdown(JObject param, string key, string currentValue)
    {
        var go = Instantiate(paramDropdownPrefab, contentPanel);
        var dropdown = go.GetComponent<ParamDropdown>();
        var options = param["options"].ToObject<string[]>();

        int selectedIndex = System.Array.IndexOf(options, currentValue);

        dropdown.Setup(param["label"]?.ToString(), options, selectedIndex, index =>
        {
            string selected = options[index];
            SettingsManager.Instance.SetOverride("audio", key, selected);
            ApplyAndSave();
        });
    }

    private void ApplyAndSave()
    {
        SettingsManager.Instance.Save();
        SettingsManager.Instance.ApplySettings();
    }
}

