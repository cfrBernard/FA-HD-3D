using UnityEngine;
using Newtonsoft.Json.Linq;

public class AudioSettingsUITest : MonoBehaviour
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
        JObject audioSettings = SettingsManagerTest.Instance.GetRawSettings()["audio"] as JObject;
        JObject audioDefaults  = SettingsManagerTest.Instance.GetDefaultSettings()["audio"] as JObject;

        foreach (var pair in audioDefaults)
        {
            string key = pair.Key;
            JObject param = pair.Value as JObject;
            string type = param["type"]?.ToString();

            switch (type)
            {
                case "slider":
                    float value = audioSettings?[key]?["default"]?.Value<float>()
                        ?? param["default"].Value<float>();
                    CreateSlider(param, key, value);
                    break;

                case "toggle":
                    bool toggleValue = audioSettings?[key]?["default"]?.Value<bool>()
                        ?? param["default"].Value<bool>();
                    CreateToggle(param, key, toggleValue);
                    break;

                case "dropdown":
                    string current = audioSettings?[key]?["default"]?.ToString()
                        ?? param["default"].ToString();
                    CreateDropdown(param, key, current);
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
                SettingsManagerTest.Instance.SetOverride("audio", key, value);
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
            SettingsManagerTest.Instance.SetOverride("audio", key, value);
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
            SettingsManagerTest.Instance.SetOverride("audio", key, selected);
            ApplyAndSave();
        });
    }

    private void ApplyAndSave()
    {
        SettingsManagerTest.Instance.Save();
        SettingsManagerTest.Instance.ApplySettings();
    }
}
