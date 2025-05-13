using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputActionAsset inputActions;

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

        inputActions = GlobalConfigs.Input.inputActions;

        UpdateBindings();
    }

    public void UpdateBindings()
    {
        if (inputActions == null)
        {
            Debug.LogError("[InputManager] inputActions is null in UpdateBindings()!");
            return;
        }

        string overridesJson = SettingsManager.Instance
            .GetSetting<string>("inputBindings", "inputActionOverridesJson");

        if (!string.IsNullOrWhiteSpace(overridesJson))
        {
            inputActions.LoadBindingOverridesFromJson(overridesJson);
            Debug.Log("[InputManager] Binding overrides applied from JSON.");
        }
        else
        {
            Debug.LogWarning("[InputManager] No binding overrides found in settings.");
        }
    }

    public void ResetAllBindings() // call by the SettingsManager Reset function
    {
        inputActions.RemoveAllBindingOverrides();
        
        Debug.Log("[InputManager] Bindings reset to default.");
    }

    public string GetBindingDisplay(string actionMap, string actionName, int bindingIndex)
    {
        var action = inputActions.FindActionMap(actionMap)?.FindAction(actionName);
        if (action == null || bindingIndex >= action.bindings.Count) return "None";
        return action.GetBindingDisplayString(bindingIndex);
    }

    public void SetBindingOverride(string actionMap, string actionName, int bindingIndex, string overridePath)
    {
        var action = inputActions.FindActionMap(actionMap)?.FindAction(actionName);
        if (action == null) return;
        action.ApplyBindingOverride(bindingIndex, overridePath);

        // Save the override via SettingsManager
        string json = inputActions.SaveBindingOverridesAsJson();
        SettingsManager.Instance.SetOverride("inputBindings", "inputActionOverridesJson", json);
        SettingsManager.Instance.Save();
    }
}

