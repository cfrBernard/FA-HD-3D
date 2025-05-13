using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

public class ControlsSettingsUI : MonoBehaviour
{
    [Header("UI Prefabs")]
    public GameObject paramSliderPrefab;
    public GameObject paramTogglePrefab;
    public GameObject paramDropdownPrefab;
    public GameObject separatorPrefab;
    public GameObject paramRebindPrefab;

    [Header("UI Panels")]
    public GameObject keyboardPanel;
    public GameObject gamepadPanel;

    private InputActionAsset inputActions;

    private void Start()
    {
        inputActions = GlobalConfigs.Input.inputActions;

        // Load binding overrides if any
        var json = SettingsManager.Instance.GetSetting<string>("inputBindings", "inputActionOverridesJson");
        if (!string.IsNullOrEmpty(json))
        {
            inputActions.LoadBindingOverridesFromJson(json);
            Debug.Log("[ControlsSettingsUI] Binding overrides loaded.");
        }
        else
        {
            Debug.LogWarning("[ControlsSettingsUI] No binding overrides found. Will use InputActionAsset defaults.");
        }

        GenerateUI("kbm", keyboardPanel.transform);
        Instantiate(separatorPrefab, keyboardPanel.transform);
        GenerateRebinds("Keyboard&Mouse", keyboardPanel);

        GenerateUI("gamepad", gamepadPanel.transform);
        Instantiate(separatorPrefab, gamepadPanel.transform);
        GenerateRebinds("Gamepad", gamepadPanel);
    }

    private void GenerateUI(string category, Transform targetPanel)
    {
        JObject metadata = SettingsManager.Instance.GetMetadataSettings()?[category] as JObject;

        if (metadata == null)
        {
            Debug.LogError($"[ControlsSettingsUI] Metadata not found or invalid for category '{category}'.");
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
                    float sliderValue = SettingsManager.Instance.GetSetting<float>(category, key);
                    CreateSlider(param, key, sliderValue, category, targetPanel);
                    break;

                case "toggle":
                    bool toggleValue = SettingsManager.Instance.GetSetting<bool>(category, key);
                    CreateToggle(param, key, toggleValue, category, targetPanel);
                    break;

                case "dropdown":
                    string dropdownValue = SettingsManager.Instance.GetSetting<string>(category, key);
                    CreateDropdown(param, key, dropdownValue, category, targetPanel);
                    break;

                default:
                    Debug.LogWarning($"[ControlsSettingsUI] Unknown type '{type}' for setting '{key}' in category '{category}'.");
                    break;
            }
        }
    }

    private void CreateSlider(JObject param, string key, float currentValue, string category, Transform targetPanel)
    {
        var go = Instantiate(paramSliderPrefab, targetPanel);
        var slider = go.GetComponent<ParamSlider>();
        slider.Setup(
            param["label"]?.ToString(),
            currentValue,
            value =>
            {
                SettingsManager.Instance.SetOverride(category, key, value);
                ApplyAndSave();
            },
            param["decimal"]?.Value<bool>() ?? false,
            param["min"]?.Value<float>() ?? 0,
            param["max"]?.Value<float>() ?? 100
        );
    }

    private void CreateToggle(JObject param, string key, bool currentValue, string category, Transform targetPanel)
    {
        var go = Instantiate(paramTogglePrefab, targetPanel);
        var toggle = go.GetComponent<ParamToggle>();
        toggle.Setup(param["label"]?.ToString(), currentValue, value =>
        {
            SettingsManager.Instance.SetOverride(category, key, value);
            ApplyAndSave();
        });
    }

    private void CreateDropdown(JObject param, string key, string currentValue, string category, Transform targetPanel)
    {
        var go = Instantiate(paramDropdownPrefab, targetPanel);
        var dropdown = go.GetComponent<ParamDropdown>();
        var options = param["options"].ToObject<string[]>();

        int selectedIndex = System.Array.IndexOf(options, currentValue);

        dropdown.Setup(param["label"]?.ToString(), options, selectedIndex, index =>
        {
            string selected = options[index];
            SettingsManager.Instance.SetOverride(category, key, selected);
            ApplyAndSave();
        });
    }

    private void GenerateRebinds(string deviceGroup, GameObject targetPanel)
    {
        foreach (var map in inputActions.actionMaps)
        {
            if (map.name != "Player") continue;

            foreach (var action in map.actions)
            {
                // Filtres
                if (action.name == "Look") continue;
                if (deviceGroup == "Gamepad" && action.name == "Move") continue;

                if (action.bindings.Any(b => b.isComposite))
                {
                    GenerateCompositeBindings(action, deviceGroup, targetPanel);
                }

                List<int> matchingIndices = new();
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];
                    if (binding.isComposite || binding.isPartOfComposite) continue;
                    if (!binding.groups.Contains(deviceGroup)) continue;
                    matchingIndices.Add(i);
                }

                if (matchingIndices.Count == 0) continue;

                var go = Instantiate(paramRebindPrefab, targetPanel.transform);
                var paramRebind = go.GetComponent<ParamRebind>();
                paramRebind.actionMap = map.name;
                paramRebind.actionName = action.name;
                paramRebind.bindingIndex1 = matchingIndices[0];
                paramRebind.bindingIndex2 = matchingIndices.Count > 1 ? matchingIndices[1] : -1;
            }
        }
    }

    private void GenerateCompositeBindings(InputAction action, string deviceGroup, GameObject targetPanel)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (!binding.isComposite) continue;

            i++; // go to the first partOfComposite

            Dictionary<string, List<int>> compositeParts = new();

            while (i < action.bindings.Count && action.bindings[i].isPartOfComposite)
            {
                var sub = action.bindings[i];
                if (!sub.groups.Contains(deviceGroup)) { i++; continue; }

                if (!compositeParts.ContainsKey(sub.name))
                    compositeParts[sub.name] = new List<int>();

                compositeParts[sub.name].Add(i);
                i++;
            }

            i--; // Fix the index after reading all partOfComposite

            // instantiates 1 prefab per direction
            foreach (var kvp in compositeParts)
            {
                string direction = kvp.Key;
                var indices = kvp.Value;

                var go = Instantiate(paramRebindPrefab, targetPanel.transform);
                var paramRebind = go.GetComponent<ParamRebind>();
                paramRebind.actionMap = action.actionMap.name;
                paramRebind.actionName = action.name;
                paramRebind.bindingIndex1 = indices.Count > 0 ? indices[0] : -1;
                paramRebind.bindingIndex2 = indices.Count > 1 ? indices[1] : -1;
                paramRebind.labelOverride = $"{action.name} ({direction})";
            }
        }
    }

    private void ApplyAndSave()
    {
        SettingsManager.Instance.Save();
        SettingsManager.Instance.ApplySettings();
    }
}
