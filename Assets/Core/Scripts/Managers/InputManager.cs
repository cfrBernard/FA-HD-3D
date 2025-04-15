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
    }

    public void UpdateBindings(SettingsData data)
    {
        inputActions.LoadBindingOverridesFromJson(data.inputBindings.inputActionOverridesJson);
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

        // Save the override in settings
        SettingsManager.Instance.GetSettingsData().inputBindings.inputActionOverridesJson =
            inputActions.SaveBindingOverridesAsJson();

        SettingsManager.Instance.Save();
    }
}
