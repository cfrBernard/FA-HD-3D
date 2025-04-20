using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParamToggle : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Button onButton;
    public Button offButton;

    public void Setup(string labelText, bool initialValue, System.Action<bool> onValueChanged)
    {
        label.text = labelText;
        UpdateVisuals(initialValue);

        offButton.onClick.AddListener(() =>
        {
            UpdateVisuals(true);
            onValueChanged(true);
        });

        onButton.onClick.AddListener(() =>
        {
            UpdateVisuals(false);
            onValueChanged(false);
        });
    }

    private void UpdateVisuals(bool isOn)
    {
        onButton.interactable = isOn;
        offButton.interactable = !isOn;
    }
}
