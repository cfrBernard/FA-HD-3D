using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class ControlsSettingsUI : MonoBehaviour
{
    [Header("Param Setup")]
    public SettingsCategory kbmCategory;
    public SettingsCategory gamepadCategory;

    public GameObject paramSliderPrefab;
    public GameObject paramTogglePrefab;
    public GameObject paramDropdownPrefab;
    public GameObject separatorPrefab;
    
    public GameObject keyboardPanel;
    public GameObject gamepadPanel;
    
    public GameObject paramRebindPrefab;

    private InputActionAsset inputActions;
    private SettingsData settingsData;

    private void Start()
    {
        inputActions = GlobalConfigs.Input.inputActions;

        var json = SettingsManager.Instance.GetSettingsData().inputBindings.inputActionOverridesJson;
        if (!string.IsNullOrEmpty(json))
        {
            inputActions.LoadBindingOverridesFromJson(json);
            Debug.Log("[ControlsSettingsUI] Binding overrides loaded.");
        }
        else
        {
            Debug.LogWarning("[ControlsSettingsUI] No binding overrides found. Will use InputActionAsset defaults.");
        }


        settingsData = SettingsManager.Instance.GetSettingsData();

        GenerateParams(kbmCategory, keyboardPanel.transform);
        Instantiate(separatorPrefab, keyboardPanel.transform);
        GenerateUI("Keyboard&Mouse", keyboardPanel);
    
        GenerateParams(gamepadCategory, gamepadPanel.transform);
        Instantiate(separatorPrefab, gamepadPanel.transform);
        GenerateUI("Gamepad", gamepadPanel);
    }

    // Generates the UI for the ControlsParam
    private void GenerateParams(SettingsCategory category, Transform targetPanel)
    {
        foreach (var param in category.parameters)
        {
            switch (param.type)
            {
                case ParamType.Slider:
                    float floatValue = (float)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);
                    var s = Instantiate(paramSliderPrefab, targetPanel);
                    s.GetComponent<ParamSlider>().Setup(
                        param.label,
                        floatValue,
                        value => {
                            ReflectionUtils.SetValueByPath(settingsData, param.key, value);
                            ApplyAndSave();
                        },
                        param.showDecimal,
                        param.minValue,
                        param.maxValue
                    );
                    break;

                case ParamType.Toggle:
                    bool boolValue = (bool)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);
                    var t = Instantiate(paramTogglePrefab, targetPanel);
                    t.GetComponent<ParamToggle>().Setup(
                        param.label,
                        boolValue,
                        value => {
                            ReflectionUtils.SetValueByPath(settingsData, param.propertyPath, value);
                            ApplyAndSave();
                        }
                    );
                    break;

                case ParamType.Dropdown:
                    int intValue = (int)ReflectionUtils.GetValueByPath(settingsData, param.propertyPath);
                    var d = Instantiate(paramDropdownPrefab, targetPanel);
                    d.GetComponent<ParamDropdown>().Setup(
                        param.label,
                        param.dropdownOptions,
                        intValue,
                        value => {
                            ReflectionUtils.SetValueByPath(settingsData, param.key, value);
                            ApplyAndSave();
                        }
                    );
                    break;
            }
        }
    }

    // Generates the UI for rebinding
    void GenerateUI(string deviceGroup, GameObject targetPanel)
    {
        foreach (var map in inputActions.actionMaps)
        {
            if (map.name != "Player") continue;

            foreach (var action in map.actions)
            {
                // Filtres
                if (action.name == "Look") continue;
                if (deviceGroup == "Gamepad" && action.name == "Move")
                    continue;

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

    // Generates the UI for rebinding (composite)
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
