using UnityEngine;
using Newtonsoft.Json.Linq;

public class VideoSettingsUITest : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform contentPanel;
    public GameObject paramSliderPrefab;
    public GameObject paramTogglePrefab;
    public GameObject paramDropdownPrefab;
    public GameObject separatorPrefab;

    private void OnEnable()
    {
        GenerateUI("video");
        Instantiate(separatorPrefab, contentPanel);
        GenerateUI("graphics");
    }

    private void GenerateUI(string category)
    {
        JObject metadata = Resources.Load<TextAsset>("MetadataSettings") is TextAsset metaAsset
            ? JObject.Parse(metaAsset.text)?[category] as JObject
            : null;

        if (metadata == null)
        {
            Debug.LogError($"[VideoSettingsUITest] Metadata not found or invalid for category '{category}'.");
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
                    float sliderValue = SettingsManagerTest.Instance.GetSetting<float>(category, key);
                    CreateSlider(param, key, sliderValue, category);
                    break;

                case "toggle":
                    bool toggleValue = SettingsManagerTest.Instance.GetSetting<bool>(category, key);
                    CreateToggle(param, key, toggleValue, category);
                    break;

                case "dropdown":
                    string dropdownValue = SettingsManagerTest.Instance.GetSetting<string>(category, key);
                    CreateDropdown(param, key, dropdownValue, category);
                    break;

                default:
                    Debug.LogWarning($"[VideoSettingsUITest] Unknown type '{type}' for setting '{key}' in category '{category}'.");
                    break;
            }
        }
    }

    private void CreateSlider(JObject param, string key, float currentValue, string category)
    {
        var go = Instantiate(paramSliderPrefab, contentPanel);
        var slider = go.GetComponent<ParamSlider>();
        slider.Setup(
            param["label"]?.ToString(),
            currentValue,
            value =>
            {
                SettingsManagerTest.Instance.SetOverride(category, key, value);
                ApplyAndSave();
            },
            param["decimal"]?.Value<bool>() ?? false,
            param["min"]?.Value<float>() ?? 0,
            param["max"]?.Value<float>() ?? 100
        );
    }

    private void CreateToggle(JObject param, string key, bool currentValue, string category)
    {
        var go = Instantiate(paramTogglePrefab, contentPanel);
        var toggle = go.GetComponent<ParamToggle>();
        toggle.Setup(param["label"]?.ToString(), currentValue, value =>
        {
            SettingsManagerTest.Instance.SetOverride(category, key, value);
            ApplyAndSave();
        });
    }

    private void CreateDropdown(JObject param, string key, string currentValue, string category)
    {
        var go = Instantiate(paramDropdownPrefab, contentPanel);
        var dropdown = go.GetComponent<ParamDropdown>();
        var options = param["options"].ToObject<string[]>();

        int selectedIndex = System.Array.IndexOf(options, currentValue);

        dropdown.Setup(param["label"]?.ToString(), options, selectedIndex, index =>
        {
            string selected = options[index];
            SettingsManagerTest.Instance.SetOverride(category, key, selected);
            ApplyAndSave();
        });
    }

    private void ApplyAndSave()
    {
        SettingsManagerTest.Instance.Save();
        SettingsManagerTest.Instance.ApplySettings();
    }
}
