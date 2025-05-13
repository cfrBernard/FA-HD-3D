using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ParamRebind : MonoBehaviour
{
    public string actionMap;
    public string actionName;
    public int bindingIndex1;
    public int bindingIndex2;
    public string labelOverride;

    public TextMeshProUGUI actionLabel;
    public Button rebindBtn1;
    public Button rebindBtn2;

    private void Start()
    {
        UpdateLabels();
        rebindBtn1.onClick.AddListener(() => Rebind(1));
        rebindBtn2.onClick.AddListener(() => Rebind(2));
    }

    void UpdateLabels()
    {
        var display1 = InputManager.Instance.GetBindingDisplay(actionMap, actionName, bindingIndex1);
        actionLabel.text = string.IsNullOrEmpty(labelOverride) ? actionName : labelOverride;
        rebindBtn1.GetComponentInChildren<TextMeshProUGUI>().text = display1.Contains("none") ? "" : display1;

        if (bindingIndex2 >= 0)
        {
            var display2 = InputManager.Instance.GetBindingDisplay(actionMap, actionName, bindingIndex2);
            rebindBtn2.GetComponentInChildren<TextMeshProUGUI>().text = display2.Contains("none") ? "" : display2;
        }
        else
        {
            rebindBtn2.GetComponentInChildren<TextMeshProUGUI>().text = "";
            rebindBtn2.interactable = false;
        }
    }

    void Rebind(int inputIndex)
    {
        var action = GlobalConfigs.Input.inputActions.FindActionMap(actionMap)?.FindAction(actionName);
        if (action == null) return;

        action.Disable();

        if (inputIndex == 1)
        {
            rebindBtn1.GetComponentInChildren<TextMeshProUGUI>().text = "Press a key...";
            action.PerformInteractiveRebinding(bindingIndex1)
                .WithCancelingThrough("<Keyboard>/escape")
                .WithControlsExcluding("<Mouse>/delta")
                .OnComplete(op =>
                {
                    op.Dispose();
                    UpdateLabels();
                    SaveBindings(action);
                })
                .Start();
        }
        else if (inputIndex == 2)
        {
            rebindBtn2.GetComponentInChildren<TextMeshProUGUI>().text = "Press a key...";
            action.PerformInteractiveRebinding(bindingIndex2)
                .WithCancelingThrough("<Keyboard>/escape")
                .WithControlsExcluding("<Mouse>/delta")
                .OnComplete(op =>
                {
                    op.Dispose();
                    UpdateLabels();
                    SaveBindings(action);
                })
                .Start();
        }
    }

    void SaveBindings(InputAction action)
    {
        var json = GlobalConfigs.Input.inputActions.SaveBindingOverridesAsJson();
        // Change the saving process to use SetOverride
        SettingsManager.Instance.SetOverride("inputBindings", "inputActionOverridesJson", json);
        SettingsManager.Instance.Save();
        action.Enable();
    }
}
