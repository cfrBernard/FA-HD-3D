using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindButton : MonoBehaviour
{
    public string actionMap;
    public string actionName;
    public int bindingIndex;

    public TextMeshProUGUI bindingLabel;
    public Button rebindBtn;

    private void Start()
    {
        UpdateLabel();
        rebindBtn.onClick.AddListener(Rebind);
    }

    void UpdateLabel()
    {
        var display = InputManager.Instance.GetBindingDisplay(actionMap, actionName, bindingIndex);
        bindingLabel.text = $"{actionName}: {display}";
    }

    void Rebind()
    {
        var action = GlobalConfigs.Input.inputActions.FindActionMap(actionMap)?.FindAction(actionName);
        if (action == null) return;

        action.Disable();

        bindingLabel.text = "Press a key...";
        action.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(op =>
            {
                op.Dispose();
                UpdateLabel();

                // SettingsData
                var json = GlobalConfigs.Input.inputActions.SaveBindingOverridesAsJson();
                SettingsManager.Instance.GetSettingsData().inputBindings.inputActionOverridesJson = json;
                SettingsManager.Instance.Save();

                action.Enable();
            })
            .Start();
    }

}

