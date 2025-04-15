using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InputSettingsUI : MonoBehaviour
{
    public GameObject keyboardPanel;
    public GameObject gamepadPanel;
    public GameObject rebindButtonPrefab;

    private InputActionAsset inputActions;

    private void Start()
    {
        inputActions = GlobalConfigs.Input.inputActions;

        var json = SettingsManager.Instance.GetSettingsData().inputBindings.inputActionOverridesJson;
        if (!string.IsNullOrEmpty(json))
        {
            inputActions.LoadBindingOverridesFromJson(json);
            Debug.Log("[InputSettingsUI] Binding overrides loaded.");
        }
        else
        {
            Debug.LogWarning("[InputSettingsUI] No binding overrides found. Will use InputActionAsset defaults.");
        }

        GenerateUI("Keyboard&Mouse", keyboardPanel);
        GenerateUI("Gamepad", gamepadPanel);
    }


    void GenerateUI(string deviceGroup, GameObject targetPanel)
    {
        foreach (var map in inputActions.actionMaps)
        {
            // actionMap Player
            if (map.name != "Player") continue;

            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];

                    // Filtrer par deviceGroup et éviter les bindings composites
                    if (!binding.groups.Contains(deviceGroup)) continue;
                    if (binding.isComposite || binding.isPartOfComposite) continue;

                    var go = Instantiate(rebindButtonPrefab, targetPanel.transform);
                    var rb = go.GetComponent<RebindButton>();
                    rb.actionMap = map.name;
                    rb.actionName = action.name;
                    rb.bindingIndex = i;
                }
            }
        }
    }


}
